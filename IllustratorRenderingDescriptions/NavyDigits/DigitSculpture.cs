﻿using Geometry;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using IllustratorRenderingDescriptions.NavyDigits.How;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits
{
    public class DigitSculpture : IDigitCreator
    {
        private readonly RectangleF _marble;
        private readonly IDigitChisleAction[] _chiselActions;

        public float StrokeWidth { get; set; } = 1;
        public float ShadowWidthPercentage { get; set; } = 1 / 8.0f;

        public string Id { get; set; }

        public int[] DigitColor { get; set; } = new int[] { 255, 255, 255 };
        public int[] ShadowColor { get; set; } = new int[] { 10, 17, 21 };

        public DigitSculpture(RectangleF marble,
            params IDigitChisleAction[] chiselActions)
        {
            _marble = marble;
            _chiselActions = chiselActions ?? Array.Empty<IDigitChisleAction>();
        }

        public string Carve()
        {
            var result = new List<DigitChiselResult>();
            foreach (var chiselAction in _chiselActions)
            {
                result.AddRange(chiselAction.GetPoints(_marble));
            }

            return ConvertToScript(result);
        }

        private string ConvertToScript(List<DigitChiselResult> chiseledOutSections)
        {
            var script = new StringBuilder();
            script.AppendLine(@"var doc = app.activeDocument;");

            var idPostfix = $"{(string.IsNullOrEmpty(Id) ? "" : $"_{Id}")}";

            //Render the paths
            script.AppendLine(CreatePath(_marble.ToPathPoints(), "doc.pathItems", $"marble{idPostfix}", DigitColor));

            for (int i = 0; i < chiseledOutSections.Count; i++)
            {
                script.AppendLine(CreatePath(chiseledOutSections[i].Points, "doc.pathItems", $"chiselSection{i}_{idPostfix}", DigitColor));
            }

            script.AppendLine(CreatePathFinderScript("Live Pathfinder Exclude", Id));

            var digitVariableName = $"digit_{Id}_path";

            script.AppendLine(SelectNamedItem(Id));
            script.AppendLine($"var {digitVariableName} = doc.selection[0];");
            script.AppendLine("app.activeDocument.selection = null;");

            var strokeColor = new int[] { 0, 0, 0 };

            if (StrokeWidth > 0)
            {
                script.AppendLine($@"if ({digitVariableName}.typename === 'PathItem') {{
{digitVariableName}.strokeWidth = {StrokeWidth};
{digitVariableName}.strokeColor = new RGBColor({strokeColor[0]},{strokeColor[1]},{strokeColor[2]});
}}
else {{
for (var i = 0; i < {digitVariableName}.pathItems.length; i++) {{
{digitVariableName}.pathItems[i].strokeWidth = {StrokeWidth};
{digitVariableName}.pathItems[i].strokeColor = new RGBColor({strokeColor[0]},{strokeColor[1]},{strokeColor[2]});
}}
}}");

            }

            script.Append(CreateShadowScript(_marble, digitVariableName, ShadowWidthPercentage, idPostfix, chiseledOutSections));

            return script.ToString();
        }

        private string CreatePathFinderScript(string pathFinderAction, string resultName, bool ungroup = true, bool group = true)
        {
            var script = new StringBuilder();

            var pathsCountVar = $"pathsCount_{Guid.NewGuid().ToString("N")}";
            var compoundPathsCountVar = $"compoundPathsCount_{Guid.NewGuid().ToString("N")}";

            script.AppendLine($@"var {pathsCountVar} = doc.pathItems.length;
var {compoundPathsCountVar} = doc.compoundPathItems.length;");

            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            script.AppendLine($@"{(group ? "app.executeMenuCommand('group')" : "")};
app.executeMenuCommand(""{pathFinderAction}"");
app.executeMenuCommand(""expandStyle"");
{(ungroup ? "app.executeMenuCommand('ungroup')" : "")};
app.activeDocument.selection[0].name = '{resultName}';
app.activeDocument.selection = null;");

            return script.ToString();
        }

        private string SelectNamedItem(string name)
        {
            var matchFoundVar = $"matchFound_{Guid.NewGuid().ToString("N")}";
            return $@"var {matchFoundVar} = false;
for (var i = 0; i < doc.compoundPathItems.length; i++) {{
if (doc.compoundPathItems[i].name == '{name}') {{ doc.compoundPathItems[i].selected = true; {matchFoundVar} = true; break; }}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.pathItems.length; i++) {{
if (doc.pathItems[i].name == '{name}') {{ doc.pathItems[i].selected = true; {matchFoundVar} = true; break; }}
}}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.groupItems.length; i++) {{
if (doc.groupItems[i].name == '{name}') {{ doc.groupItems[i].selected = true; {matchFoundVar} = true; break; }}
}}
}}";
        }

        private string FindItemRefByName(string name, StringBuilder scriptBuilder)
        {
            var variableName = $"pathMatch_{Guid.NewGuid().ToString("N")}";
            var matchFoundVar = $"matchFound_{Guid.NewGuid().ToString("N")}";
            var result = $@"var {matchFoundVar} = false;
var {variableName};
for (var i = 0; i < doc.compoundPathItems.length; i++) {{
if (doc.compoundPathItems[i].name == '{name}') {{ {variableName} = doc.compoundPathItems[i]; {matchFoundVar} = true; break; }}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.pathItems.length; i++) {{
if (doc.pathItems[i].name == '{name}') {{ {variableName} = doc.pathItems[i]; {matchFoundVar} = true; break; }}
}}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.groupItems.length; i++) {{
if (doc.groupItems[i].name == '{name}') {{{variableName} = doc.groupItems[i]; {matchFoundVar} = true; break; }}
}}
}}";

            scriptBuilder.AppendLine(result);
            return variableName;
        }

        private string CreateShadowScript(RectangleF marble,
            string digitVariableName,
            float dimensionPercentage,
            string idPostfix,
            List<DigitChiselResult> chiseledOutSections,
            float shadowAngle = 45)
        {
            var script = new StringBuilder();

            //var shadowsCreator = new DigitShadowLinesCreator(new ShadowCreator(dimensionPercentage, shadowAngle)) { StrokeWidth = StrokeWidth };
            //var updatedShadowPaths = shadowsCreator.CreateShadowPaths(marble, chiseledOutSections).ToList();

            var shadowsCreator = new DigitShadowLinesCreator(new ShadowCreator(dimensionPercentage, shadowAngle)) { StrokeWidth = StrokeWidth };
            var updatedShadowPaths = shadowsCreator.CreateShadowPaths(marble, chiseledOutSections).ToList();

            var shadowPathsName = $"shadows_{idPostfix}";

            for (int i = 0; i < updatedShadowPaths.Count; i++)
            {
                var duplicateVar = $"dup_digit_{Guid.NewGuid().ToString("N")}";
                script.AppendLine($@"var {duplicateVar} = {digitVariableName}.duplicate();
{duplicateVar}.name = '{duplicateVar}'
{duplicateVar}.selected = true;");

                var itemName = $"{shadowPathsName}_{i}";
                script.AppendLine(CreatePath(updatedShadowPaths[i], "doc.pathItems", $"{itemName}_original", ShadowColor));

                //Remove any parts of the shadows that overlap the digit
                script.AppendLine(CreatePathFinderScript("Live Pathfinder Minus Back", itemName));
                //script.AppendLine("app.executeMenuCommand(\"Live Pathfinder Exclude\");");
                //script.AppendLine("app.activeDocument.selection = null;");

                //Make sure to get rid of the stroke that gets added after running the path finder operation
                var updatedShadowRef = FindItemRefByName(itemName, script);
                script.AppendLine($@"{updatedShadowRef}.strokeWidth = 0;
{updatedShadowRef}.strokeColor = new NoColor();");
            }

            //script.AppendLine(CreateCompoundPath(updatedShadowPaths, "doc.compoundPathItems", shadowPathsName, x => $"shadow_line{x}_{idPostfix}", isBlack: true));



            //var removeShadowPaths = removeShadowLines
            //    .Select(removeShadowLine => new[] { removeShadowLine.Start, removeShadowLine.End, new PointF(removeShadowLine.End.X + offsetX, removeShadowLine.End.Y + offsetY), new PointF(removeShadowLine.Start.X + offsetX, removeShadowLine.Start.Y + offsetY) });

            //var removeShadowPathsName = $"remove_shadows_{idPostfix}";
            //script.AppendLine(CreateCompoundPath(removeShadowPaths, "doc.compoundPathItems", removeShadowPathsName, x => $"remove_shadow_line{x}_{idPostfix}", isBlue: true));

            //var removeShadowLinesGroupName = $"{Id}_remove_shadow_lines";
            //script.AppendLine(CreatePathFinderScript("Live Pathfinder Add", removeShadowLinesGroupName, false));

            //var shadowPaths = shadowLines
            //    .Select(shadowLine => new[] { shadowLine.Start, shadowLine.End, new PointF(shadowLine.End.X + offsetX, shadowLine.End.Y + offsetY), new PointF(shadowLine.Start.X + offsetX, shadowLine.Start.Y + offsetY) });

            //var shadowPathsName = $"shadows_{idPostfix}";
            //script.AppendLine(CreateCompoundPath(shadowPaths, "doc.compoundPathItems", shadowPathsName, x => $"shadow_line{x}_{idPostfix}", isBlack: true));

            //var shadowLinesGroupName = $"{Id}_shadow_lines";
            //script.AppendLine(CreatePathFinderScript("Live Pathfinder Add", shadowLinesGroupName, false));

            //script.AppendLine(SelectNamedItem(shadowPathsName));
            //script.AppendLine(SelectNamedItem(removeShadowPathsName));

            //script.AppendLine(CreatePathFinderScript("Live Pathfinder Minus Back", ""));
            //script.AppendLine("app.executeMenuCommand(\"Live Pathfinder Minus Back\");");
            script.AppendLine("app.activeDocument.selection = null;");

            return script.ToString();
        }

        private string CreatePath(PointD[] points, string pathItems, string variableName, int[] color, bool isClosed = true)
        {
            return $@"var {variableName} = {pathItems}.add();
{variableName}.setEntirePath({CreateJavaScriptArray(points)});
{variableName}.closed = {isClosed.ToString().ToLower()};
{variableName}.selected = true;
{variableName}.fillColor = new RGBColor();
{variableName}.fillColor.red = {color[0]};
{variableName}.fillColor.green = {color[1]};
{variableName}.fillColor.blue = {color[2]};
{variableName}.name = '{variableName}'";
        }

        private string CreatePath(PointF[] points, string pathItems, string variableName, int[] color, bool isClosed = true)
        {
            return $@"var {variableName} = {pathItems}.add();
{variableName}.setEntirePath({CreateJavaScriptArray(points)});
{variableName}.closed = {isClosed.ToString().ToLower()};
{variableName}.selected = true;
{variableName}.fillColor = new RGBColor();
{variableName}.fillColor.red = {color[0]};
{variableName}.fillColor.green = {color[1]};
{variableName}.fillColor.blue = {color[2]};
{variableName}.name = '{variableName}'";
        }

        private string CreateCompoundPath(IEnumerable<PointF[]> paths, string compoundPathItems, string compoundPathName, Func<int, string> pathNameFunc, bool isClosed = true, bool isBlack = false, bool isBlue = false)
        {
            var red = isBlack ? 0 : (isBlue ? 0 : 255);
            var green = 0;
            var blue = isBlack ? 0 : (isBlue ? 255 : 0);

            var script = new StringBuilder();

            var compoundPathVar = $"compoundPathItem_{Guid.NewGuid().ToString("N")}";
            script.AppendLine($@"var {compoundPathVar} = {compoundPathItems}.add();
{compoundPathVar}.name = '{compoundPathName}';");

            var index = 1;
            foreach (var path in paths)
            {
                var pathItemVar = $"pathItem_{Guid.NewGuid().ToString("N")}";
                script.AppendLine($@"var {pathItemVar} = {compoundPathVar}.pathItems.add()
{pathItemVar}.setEntirePath({CreateJavaScriptArray(path)});
{pathItemVar}.closed = {isClosed.ToString().ToLower()};
{pathItemVar}.fillColor = new RGBColor();
{pathItemVar}.fillColor.red = {red};
{pathItemVar}.fillColor.green = {green};
{pathItemVar}.fillColor.blue = {blue};
{pathItemVar}.name = '{pathNameFunc(index)}'");

                index++;
            }

            script.AppendLine($"{compoundPathVar}.selected = true;");

            return script.ToString();
        }

        private string CreateJavaScriptArray(PointD[] points)
        {
            //Make sure to slip the points vertically since illustrator renders towards
            //the top of the screen as y increases rather than the standard programming
            //way of having increasing y render towards the bottom of the screen
            return $"[{string.Join(",", points.Select(x => $"[{x.X}, {-x.Y}]"))}]";
        }

        private string CreateJavaScriptArray(PointF[] points)
        {
            //Make sure to slip the points vertically since illustrator renders towards
            //the top of the screen as y increases rather than the standard programming
            //way of having increasing y render towards the bottom of the screen
            return $"[{string.Join(",", points.Select(x => $"[{x.X}, {-x.Y}]"))}]";
        }
    }
}

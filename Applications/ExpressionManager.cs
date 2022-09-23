using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout;
using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.InputDescriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout
{
    public class ExpressionManager
    {
        private readonly ExpressionDisplaySettings _settings;

        public ExpressionManager(ExpressionDisplaySettings settings)
        {
            _settings = settings;
        }

        public ILayoutResults Render(IExpressionComponent expression)
        {
            var layout = new MatrixExpressionLayout(_settings.TextSettings, _settings.MatrixSettings);
            return layout.Layout(expression);
        }
    }

    public class ExpressionDisplaySettings
    {
        public readonly TextDisplayDescription TextSettings;
        public readonly MatrixLayoutDescription MatrixSettings;

        public ExpressionDisplaySettings(TextDisplayDescription textSettings,
            MatrixLayoutDescription matrixSettings)
        {
            TextSettings = textSettings;
            MatrixSettings = matrixSettings;
        }
    }
}

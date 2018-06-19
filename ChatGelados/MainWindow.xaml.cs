using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatGelados
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bot bot = new Bot();

        public MainWindow()
        {
            InitializeComponent();
            bot.Bot_GotResposta += Bot_GotResposta;
            Bot_GotResposta(bot.respostas);

        }

        #region Metodos privados
        private async void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock1 = new TextBlock();
            Label label1 = new Label();

            textBlock1.HorizontalAlignment = HorizontalAlignment.Right;

            textBlock1.VerticalAlignment = VerticalAlignment.Bottom;

            textBlock1.Background = Brushes.LightGreen; // new LinearGradientBrush(Colors.Green, Colors.Green, 0);

            textBlock1.Foreground = Brushes.Black;

            textBlock1.IsEnabled = false;

            label1.Content = "  ";
            textBlock1.Text = TextBoxMensagem.Text;

            StackPanelChat.Children.Add(label1);
            StackPanelChat.Children.Add(textBlock1);
            ScrollViewerChat.ScrollToBottom();

            await bot.ProcuraResposta(TextBoxMensagem.Text);
        }

        private void Bot_GotResposta(List<string> respostas)
        {
            TextBlock textBlock2;

            Label label2;

            foreach (string resposta in respostas)
            {
                textBlock2 = new TextBlock();

                label2 = new Label();

                textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock2.VerticalAlignment = VerticalAlignment.Bottom;
                textBlock2.Background = Brushes.LightGray; //new LinearGradientBrush(Colors.LightGray, Colors.LightGray, 0);
                textBlock2.Foreground = Brushes.Black;
                textBlock2.IsEnabled = false;
                textBlock2.TextWrapping = TextWrapping.Wrap;
                label2.Content = "  ";

                textBlock2.Text = resposta;

                StackPanelChat.Children.Add(label2);
                StackPanelChat.Children.Add(textBlock2);
                ScrollViewerChat.ScrollToBottom();
            }
        }

        private void TextBoxMensagem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonSend_Click(ButtonSend, null);
            }
        }
        #endregion
    }
}

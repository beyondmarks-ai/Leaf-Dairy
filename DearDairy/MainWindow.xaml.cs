using System.Windows;
using System.Windows.Media;

namespace DearDairy;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void NewEntry_Click(object sender, RoutedEventArgs e)
    {
        NewEntryOverlay.Visibility = Visibility.Visible;
    }

    private void NewEntryComposer_CloseRequested(object? sender, EventArgs e) =>
        NewEntryOverlay.Visibility = Visibility.Collapsed;

    private void Today_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("You are already viewing today's writing timeline.", "Leaf Dairy",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void QuickNote_GotFocus(object sender, RoutedEventArgs e)
    {
        if (QuickNote.Text != "Jot down a quick idea...") return;
        QuickNote.Clear();
        QuickNote.Foreground = new SolidColorBrush(Color.FromRgb(213, 218, 232));
    }
}

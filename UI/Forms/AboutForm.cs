using System.Diagnostics;
using System.Reflection;

namespace nRun.UI.Forms;

public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        LoadVersionInfo();
    }

    private void LoadVersionInfo()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        lblVersion.Text = $"Version {version?.Major}.{version?.Minor}.{version?.Build}";
    }

    private void LnkFacebook_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        OpenUrl("https://fb.com/rubel.social");
    }

    private void LnkPhone_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        OpenUrl("tel:+8801760002332");
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private static void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch
        {
            // Ignore errors opening URL
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        // Cleanup event handlers
        lnkFacebook.LinkClicked -= LnkFacebook_LinkClicked;
        lnkPhone.LinkClicked -= LnkPhone_LinkClicked;
        btnClose.Click -= BtnClose_Click;
    }
}

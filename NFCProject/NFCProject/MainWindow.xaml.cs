﻿using System;
using System.Collections;
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
using Parse;
using Microsoft.Win32;
using System.IO;

namespace NFCProject {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
      List<ParseObject> m_ListObject;
      BitmapImage m_bitmap1, m_bitmap2;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void frmMain_Activated(object sender, EventArgs e)
    {

    }

    private async void frmMain_Loaded(object sender, RoutedEventArgs e)
    {
        ParseAnalytics.TrackAppOpenedAsync();

        m_ListObject = new List<ParseObject>();

        //var testObject = new ParseObject("Windows");
        //testObject["WPF"] = "okay";
        //await testObject.SaveAsync();

        await RefreshList();
        
        
    }

    private async Task RefreshList()
    {
        int nCount1= lst_Recent.Items.Count;
        for (int i = 0; i < nCount1; ++i)
            lst_Recent.Items.RemoveAt(0);

        int nCount2 = lst_Regist.Items.Count;
        for (int i = 0; i < nCount2; ++i)
            lst_Regist.Items.RemoveAt(0);

        var query = ParseObject.GetQuery("NFC_List").WhereEqualTo("linked_user", "june");
        IEnumerable<ParseObject> results = await query.FindAsync();

        IEnumerator e1 = results.GetEnumerator();
        while (e1.MoveNext())
        {
            ParseObject obj = (ParseObject)(e1.Current);
            m_ListObject.Add(obj);
            lst_Recent.Items.Add(obj.Get<string>("NFC_id"));
        }

        var query2 = ParseObject.GetQuery("NFC_reg").WhereEqualTo("linked_user", "june");
        IEnumerable<ParseObject> results2 = await query2.FindAsync();

        IEnumerator e2 = results2.GetEnumerator();
        while (e2.MoveNext())
        {
            ParseObject obj = (ParseObject)(e2.Current);
            //m_ListObject.Add(obj);
            lst_Regist.Items.Add(obj.Get<string>("NFC_id"));
        }
    }

    private async void btn_refresh_Click(object sender, RoutedEventArgs e)
    {
        await RefreshList();
    }

      // 선택이 바뀌면
    private void lst_Recent_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ParseObject obj = m_ListObject[lst_Recent.SelectedIndex];
        
        DateTime? timeCreated = obj.CreatedAt;
        txt_date.Text= timeCreated.ToString();

        txt_id.Text= obj.Get<string>("NFC_id");

    }

      // 등록
    private async void btn_regist_Click_1(object sender, RoutedEventArgs e)
    {
        if (txt_caption1.Text.Equals(""))
        {
            MessageBoxResult result = MessageBox.Show("Please input caption.");
            return;
        }
        else if (txt_image1.Text.Equals("") || txt_image2.Text.Equals(""))
        {
            MessageBoxResult result = MessageBox.Show("Please select images.");
            return;
        }

        byte[] data1 = this.ConvertImageToByte(m_bitmap1);
        ParseFile file1 = new ParseFile("1.png", data1);
        await file1.SaveAsync();

        byte[] data2 = this.ConvertImageToByte(m_bitmap2);
        ParseFile file2 = new ParseFile("2.png", data2);
        await file2.SaveAsync();

        var NFCreg = new ParseObject("NFC_reg");
        NFCreg["NFC_id"] = txt_id.Text;
        NFCreg["linked_user"] = "june";
        NFCreg["Caption"] = txt_caption1.Text;
        NFCreg["File1"] = file1;
        NFCreg["File2"] = file2;
        await NFCreg.SaveAsync();

        MessageBox.Show("Regist finish!");
    }

    private void btn_remove_Click_1(object sender, RoutedEventArgs e)
    {

    }


    private void SetImage(int nNumber)
    {
        OpenFileDialog openDialog = new OpenFileDialog();
        if (openDialog.ShowDialog() == true)
        {
            if (File.Exists(openDialog.FileName))
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(openDialog.FileName, UriKind.RelativeOrAbsolute));

                if (nNumber == 1)
                {
                    m_bitmap1 = bitmapImage;
                    img_1.Source = bitmapImage;
                    txt_image1.Text = openDialog.FileName;
                }
                if (nNumber == 2)
                {
                    m_bitmap2 = bitmapImage;
                    img_2.Source = bitmapImage;
                    txt_image2.Text = openDialog.FileName;
                }

            }
        }
    }

    private void btn_image1_Click(object sender, RoutedEventArgs e)
    {
        this.SetImage(1);
    }

    private void btn_image2_Click(object sender, RoutedEventArgs e)
    {
        this.SetImage(2);
    }

    private byte[] ConvertImageToByte(BitmapImage bitmap)
    {
        byte[] data;
        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using (MemoryStream ms = new MemoryStream())
        {
            encoder.Save(ms);
            data = ms.ToArray();
        }

        return data;
    }

  }
}

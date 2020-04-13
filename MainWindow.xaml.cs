using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace wpf_attachments
{
    public partial class MainWindow : Window
    {
        public class Payload
        {
            public string url { get; set; }
            public string name { get; set; }
        }

        public class DownloadedFile
        {
            public string FileThumbnail { get; set; }
            public string FilePath { get; set; }
            public string FileName { get; set; }
        }

        public ObservableCollection<DownloadedFile> FilesCollection { get; set; }
        public string _attachmentPath;

        public MainWindow()
        {
            GetAttachments();
        }

        public void GetAttachments()
        {
            _attachmentPath = "C:/temp/attachments/";

            var downloadStrings = new List<Payload>();
            downloadStrings.Add(new Payload() { url = "https://extend.castsoftware.com/resources/com.castsoftware.wpf.png", name = "Picture.png" });
            downloadStrings.Add(new Payload() { url = "https://www.w3.org/TR/PNG/iso_8859-1.txt", name = "Text file.txt" });
            downloadStrings.Add(new Payload() { url = "https://file-examples.com/wp-content/uploads/2017/02/file_example_XLS_10.xls", name = "Xls file.xls" });

            foreach (Payload file in downloadStrings)
            {
                WebClient webClient = new WebClient();
                {
                    webClient.DownloadFile(file.url, string.Format(_attachmentPath + file.name));
                }
            }

            //get those files and load them to UI
            CollectDownloadedFiles();
        }

        public void CollectDownloadedFiles()
        {
            FilesCollection = new ObservableCollection<DownloadedFile>();

            var folder = new DirectoryInfo(_attachmentPath);
            var downloadedAttachments = folder.GetFiles("*");

            foreach (var file in downloadedAttachments)
            {
                //if file == picture, show thumbnail as it is
                if (System.IO.Path.GetExtension(file.FullName) == ".jpg" || System.IO.Path.GetExtension(file.FullName) == ".png" ||
                    System.IO.Path.GetExtension(file.FullName) == ".gif" || System.IO.Path.GetExtension(file.FullName) == ".bmp" ||
                    System.IO.Path.GetExtension(file.FullName) == ".ico")
                {

                    FilesCollection.Add(new DownloadedFile()
                    {
                        FileThumbnail = file.FullName,
                        FilePath = file.FullName,
                        FileName = file.Name
                    });

                }

                //if file != picture, set txt or log thumbnail
                else if (System.IO.Path.GetExtension(file.FullName) == ".txt" || System.IO.Path.GetExtension(file.FullName) == ".log")
                {

                    FilesCollection.Add(new DownloadedFile()
                    {
                        FileThumbnail = "Images/notepad.jpg",
                        FilePath = file.FullName,
                        FileName = file.Name
                    });

                }

                //if file is MS Word file, set docx thumbnail
                else if (System.IO.Path.GetExtension(file.FullName) == ".docx")
                {
                    FilesCollection.Add(new DownloadedFile()
                    {
                        FileThumbnail = "Images/doc_word.png",
                        FilePath = file.FullName,
                        FileName = file.Name
                    });
                }

                //if file is excel document, set excel thumbnail
                else if (System.IO.Path.GetExtension(file.FullName) == ".xls" || System.IO.Path.GetExtension(file.FullName) == ".csv")
                {
                    FilesCollection.Add(new DownloadedFile()
                    {
                        FileThumbnail = "Images/excel.png",
                        FilePath = file.FullName,
                        FileName = file.Name
                    });
                }

                //if file has none of our extensions, set the default file thumbnail
                else
                {
                    FilesCollection.Add(new DownloadedFile()
                    {
                        FileThumbnail = "Images/doc_unknown.png",
                        FilePath = file.FullName,
                        FileName = file.Name
                    });
                }
            }
        }

        public void openFile(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                foreach (object o in ListBoxFiles.SelectedItems)
                    Process.Start((o as DownloadedFile).FilePath);
            }
        }
    }
}

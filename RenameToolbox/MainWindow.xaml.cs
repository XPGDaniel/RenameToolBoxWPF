using Microsoft.Win32;
using RenameToolbox.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RenameToolbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private XMLHelper xmlH = new XMLHelper();
        public bool MakeRAR = false, BMP2PNG = false;
        private enum MoveDirection { Up = -1, Down = 1 };
        public ObservableCollection<Rule> rules = new ObservableCollection<Rule>();
        public ObservableCollection<ItemToRename> Targets = new ObservableCollection<ItemToRename>();
        
        private void btn_Preview_Click(object sender, RoutedEventArgs e)
        {

            if (lView_Rules.Items.Count > 0 && lView_TargetList.Items.Count > 0)
            {
                lView_TargetList.ItemsSource = null;
                List<ItemToRename> newFilenameList = new List<ItemToRename>();
                foreach (ItemToRename vi in Targets)
                {
                    ItemToRename ftr = new ItemToRename()
                    {
                        Path = vi.Path,
                        Before = vi.Before
                    };
                    newFilenameList.Add(ftr);
                }
                if (rules.Any())
                {
                    foreach (Rule rule in rules)
                    {
                        if (rule.Enable)
                            switch (rule.Target)
                            {
                                case GlobalConst.TARGETTYPE_FILENAME:
                                    switch (rule.Mode)
                                    {
                                        case GlobalConst.MODETYPE_REMOVE:
                                            newFilenameList = RuleHandler.RemoveFileName(newFilenameList, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_REPLACE:
                                            newFilenameList = RuleHandler.ReplaceFileName(newFilenameList, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_PREFIX:
                                            newFilenameList = RuleHandler.PrefixFileName(newFilenameList, rule.p1);
                                            break;
                                        case GlobalConst.MODETYPE_SUFFIX:
                                            newFilenameList = RuleHandler.SuffixFileName(newFilenameList, rule.p1);
                                            break;
                                        case GlobalConst.MODETYPE_UPPERCASE:
                                            newFilenameList = RuleHandler.ChangeFileNameCase(newFilenameList, GlobalConst.MOVE_UP, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_LOWERCASE:
                                            newFilenameList = RuleHandler.ChangeFileNameCase(newFilenameList, GlobalConst.MOVE_DOWN, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_INSERT:
                                            newFilenameList = RuleHandler.InsertIntoFileName(newFilenameList, rule.p1, rule.p2, rule.Sub);
                                            break;
                                        case GlobalConst.MODETYPE_BMP2PNG:
                                            BMP2PNG = true;
                                            break;
                                    }
                                    break;
                                case GlobalConst.TARGETTYPE_FILEEXTENSION:
                                    switch (rule.Mode)
                                    {
                                        case GlobalConst.MODETYPE_REMOVE:
                                            newFilenameList = RuleHandler.RemoveFileExtension(newFilenameList, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_REPLACE:
                                            newFilenameList = RuleHandler.ReplaceFileExtension(newFilenameList, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_PREFIX:
                                            newFilenameList = RuleHandler.PrefixFileExtension(newFilenameList, rule.p1);
                                            break;
                                        case GlobalConst.MODETYPE_SUFFIX:
                                            newFilenameList = RuleHandler.SuffixFileExtension(newFilenameList, rule.p1);
                                            break;
                                        case GlobalConst.MODETYPE_UPPERCASE:
                                            newFilenameList = RuleHandler.ChangeFileExtensionCase(newFilenameList, GlobalConst.MOVE_UP, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_LOWERCASE:
                                            newFilenameList = RuleHandler.ChangeFileExtensionCase(newFilenameList, GlobalConst.MOVE_DOWN, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_INSERT:
                                            newFilenameList = RuleHandler.InsertIntoFileExtension(newFilenameList, rule.p1, rule.p2, rule.Sub);
                                            break;
                                    }
                                    break;
                                case GlobalConst.TARGETTYPE_FOLDERNAME:
                                    switch (rule.Mode)
                                    {
                                        case GlobalConst.MODETYPE_REMOVE:
                                            newFilenameList = RuleHandler.RemoveFolderName(newFilenameList, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_REPLACE:
                                            newFilenameList = RuleHandler.ReplaceFolderName(newFilenameList, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_PREFIX:
                                            newFilenameList = RuleHandler.PrefixFolderName(newFilenameList, rule.p1);
                                            break;
                                        case GlobalConst.MODETYPE_SUFFIX:
                                            newFilenameList = RuleHandler.SuffixFolderName(newFilenameList, rule.p1);
                                            break;
                                        case GlobalConst.MODETYPE_UPPERCASE:
                                            newFilenameList = RuleHandler.ChangeFolderNameCase(newFilenameList, GlobalConst.MOVE_UP, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_LOWERCASE:
                                            newFilenameList = RuleHandler.ChangeFolderNameCase(newFilenameList, GlobalConst.MOVE_DOWN, rule.p1, rule.p2);
                                            break;
                                        case GlobalConst.MODETYPE_INSERT:
                                            newFilenameList = RuleHandler.InsertIntoFolderName(newFilenameList, rule.p1, rule.p2, rule.Sub);
                                            break;
                                        case GlobalConst.MODETYPE_MAKERAR:
                                            MakeRAR = true;
                                            break;
                                    }
                                    break;
                            }
                    }
                    for (int i = 0; i < Targets.Count; i++)
                    {
                        int index = Targets.IndexOf(Targets.Where(X => X.Path == newFilenameList[i].Path).FirstOrDefault());
                        Targets[index].After = newFilenameList[i].Before;
                    }
                }
                newFilenameList.Clear();
                lView_TargetList.ItemsSource = Targets;
                lView_TargetList.Items.Refresh();
            }
        }

        private void btn_GO_Click(object sender, RoutedEventArgs e)
        {
            btn_Preview_Click(null, null);
            if (lView_Rules.Items.Count > 0 && lView_TargetList.Items.Count > 0)
            {
                lView_TargetList.ItemsSource = null;
                List<ItemToRename> RenameList = new List<ItemToRename>();
                foreach (ItemToRename vi in Targets)
                {
                    ItemToRename ftr = new ItemToRename()
                    {
                        Path = vi.Path,
                        Before = vi.Before,
                        After = vi.After
                    };
                    RenameList.Add(ftr);
                }
                if (RenameList.Any())
                {

                    switch (cbox_TargetType.SelectedItem.ToString())
                    {
                        case GlobalConst.TARGETTYPE_FOLDERNAME:
                            foreach (ItemToRename fileCandidate in RenameList)
                            {
                                if (Directory.Exists(fileCandidate.Path))
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(Directory.GetParent(fileCandidate.Path).FullName + "\\" + fileCandidate.After))
                                        {
                                            if (fileCandidate.Path != Directory.GetParent(fileCandidate.Path).FullName + "\\" + fileCandidate.After)
                                            {
                                                Directory.Move(fileCandidate.Path, Directory.GetParent(fileCandidate.Path).FullName + "\\" + fileCandidate.After);
                                                fileCandidate.Result = GlobalConst.RESULT_RENAME_OK;
                                            }
                                            if (MakeRAR)
                                            {
                                                using (Process p = new Process())
                                                {
                                                    p.StartInfo.FileName = GlobalConst.PATH_WINRAR;
                                                    //p.StartInfo.RedirectStandardOutput = true;
                                                    //p.StartInfo.RedirectStandardError = true;
                                                    p.StartInfo.UseShellExecute = false;
                                                    p.StartInfo.CreateNoWindow = false; //Default:true
                                                    p.StartInfo.Arguments = " a -ep1 -r \"" + Directory.GetParent(fileCandidate.Path).FullName + "\\" + fileCandidate.After + ".rar\" \"" + Directory.GetParent(fileCandidate.Path).FullName + "\\" + fileCandidate.After + "\\*\" -rr3p";
                                                    p.Start();
                                                    //string stdoutx = p.StandardOutput.ReadToEnd();
                                                    //string stderrx = p.StandardError.ReadToEnd();
                                                    p.WaitForExit();
                                                    //Console.WriteLine("Exit code : {0}", p.ExitCode);
                                                    //Console.WriteLine("Stdout : {0}", stdoutx);
                                                    //Console.WriteLine("Stderr : {0}", stderrx);
                                                    fileCandidate.Result += GlobalConst.RESULT_RAR_OK;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            fileCandidate.Result = GlobalConst.RESULT_INVALID_NEW_FOLDERNAME;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        fileCandidate.Result = GlobalConst.RESULT_RENAME_FAIL;
                                    }
                                }
                            }
                            break;
                        default:
                            foreach (ItemToRename fileCandidate in RenameList)
                            {
                                if (File.Exists(fileCandidate.Path))
                                {
                                    try
                                    {
                                        FileInfo fi = new FileInfo(fileCandidate.Path);
                                        if (!string.IsNullOrEmpty(System.IO.Path.GetFileNameWithoutExtension(fileCandidate.After)) && !string.IsNullOrEmpty(System.IO.Path.GetExtension(fileCandidate.After)))
                                        {
                                            if (fi.FullName != fi.FullName.Replace(fileCandidate.Before, fileCandidate.After))
                                            {
                                                File.Move(fi.FullName, fi.FullName.Replace(fileCandidate.Before, fileCandidate.After));
                                            }
                                            fileCandidate.Result = GlobalConst.RESULT_RENAME_OK;
                                            if (BMP2PNG && System.IO.Path.GetExtension(fileCandidate.Path).ToLowerInvariant() == @".bmp")
                                            {
                                                BitmapSource SourceBMP = new BitmapImage(new Uri(Directory.GetParent(fileCandidate.Path).FullName + "\\" + fileCandidate.After));
                                                using (var fileStream = new FileStream(Directory.GetParent(fileCandidate.Path).FullName + "\\" + System.IO.Path.GetFileNameWithoutExtension(fileCandidate.After) + ".png", FileMode.Create))
                                                {
                                                    BitmapEncoder encoder = new PngBitmapEncoder();
                                                    encoder.Frames.Add(BitmapFrame.Create(SourceBMP));
                                                    encoder.Save(fileStream);
                                                }
                                                fileCandidate.Result += "-" + GlobalConst.RESULT_PNG_OK;
                                            }
                                        }
                                        else
                                        {
                                            fileCandidate.Result = GlobalConst.RESULT_INVALID_NEW_FILENAME_FILEEXTENSION;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        fileCandidate.Result = GlobalConst.RESULT_RENAME_FAIL;
                                    }
                                }
                            }
                            break;
                    }
                }
                for (int i = 0; i < RenameList.Count; i++)
                {
                    int index = Targets.IndexOf(Targets.Where(X => X.Path == RenameList[i].Path).FirstOrDefault());
                    Targets[index].Result = RenameList[i].Result;
                }
                RenameList.Clear();
                lView_TargetList.ItemsSource = Targets;
                lView_TargetList.Items.Refresh();
                btn_Undo.IsEnabled = true;
            }
            MessageBox.Show("Done!");
            MakeRAR = false; BMP2PNG = false;
        }

        private void btn_Undo_Click(object sender, RoutedEventArgs e)
        {
            if (lView_TargetList.Items.Count > 0)
            {
                List<ItemToRename> UndoList = new List<ItemToRename>();
                foreach (ItemToRename vi in lView_TargetList.Items)
                {
                    ItemToRename ftr = new ItemToRename()
                    {
                        Path = vi.Path,
                        Before = vi.Before,
                        After = vi.After,
                        Result = vi.Result
                    };
                    UndoList.Add(ftr);
                }
                if (UndoList.Any())
                {
                    switch (cbox_TargetType.SelectedItem.ToString())
                    {
                        case GlobalConst.TARGETTYPE_FOLDERNAME:
                            foreach (ItemToRename fileCandidate in UndoList)
                            {
                                string CurrentNewFolderName = Directory.GetParent(fileCandidate.Path).FullName + "\\" + fileCandidate.After;
                                if (Directory.Exists(CurrentNewFolderName))
                                {
                                    try
                                    {
                                        Directory.Move(CurrentNewFolderName, fileCandidate.Path);
                                        fileCandidate.Result = GlobalConst.RESULT_UNDO_OK;
                                    }
                                    catch (Exception)
                                    {
                                        fileCandidate.Result = GlobalConst.RESULT_UNDO_FAIL;
                                    }
                                }
                            }
                            break;
                        default:
                            foreach (ItemToRename fileCandidate in UndoList)
                            {
                                string CurrentNewFileName = fileCandidate.Path.Replace(fileCandidate.Before, fileCandidate.After);
                                if (File.Exists(CurrentNewFileName))
                                {
                                    try
                                    {
                                        File.Move(CurrentNewFileName, CurrentNewFileName.Replace(fileCandidate.After, fileCandidate.Before));
                                        fileCandidate.Result = GlobalConst.RESULT_UNDO_OK;
                                    }
                                    catch (Exception)
                                    {
                                        fileCandidate.Result = GlobalConst.RESULT_UNDO_FAIL;
                                    }
                                }
                            }
                            break;
                    }
                    for (int i = 0; i < UndoList.Count; i++)
                    {
                        int index = Targets.IndexOf(Targets.Where(X => X.Path == UndoList[i].Path).FirstOrDefault());
                        Targets[index].Result = UndoList[i].Result;
                        //int fooItem = lView_TargetList.Items.IndexOf(UndoList[i].Path);
                        //((ItemToRename)lView_TargetList.Items[i]).Result = UndoList[i].Result;
                    }
                }
                UndoList.Clear();
                lView_TargetList.Items.Refresh();
                btn_Undo.IsEnabled = false;
            }
        }
        #region lView_TargetList
        private void lView_TargetList_Drop(object sender, DragEventArgs e)
        {
            foreach (string s in (string[])e.Data.GetData(DataFormats.FileDrop, false))
            {
                if (Directory.Exists(s))
                {//Add files from folder
                    switch (cbox_TargetType.SelectedItem.ToString())
                    {
                        case GlobalConst.TARGETTYPE_FOLDERNAME:
                            DirectoryInfo di = new DirectoryInfo(s);
                            if (Targets.IndexOf(Targets.Where(X => X.Path == di.FullName).FirstOrDefault()) < 0)
                                Targets.Add(new ItemToRename { Path = di.FullName, Before = di.Name, After = GlobalConst.EMPTY_STRING, Result = GlobalConst.EMPTY_STRING });
                            break;
                        default:
                            List<string> filepaths = new List<string>();
                            filepaths.AddRange(Directory.GetFiles(s, "*.*", SearchOption.AllDirectories));
                            foreach (string item in filepaths)
                            {
                                FileInfo fi = new FileInfo(item);
                                if (Targets.IndexOf(Targets.Where(X => X.Path == fi.FullName).FirstOrDefault()) < 0)
                                    Targets.Add(new ItemToRename { Path = fi.FullName, Before = fi.Name, After = GlobalConst.EMPTY_STRING, Result = GlobalConst.EMPTY_STRING });
                            }
                            break;
                    }
                }
                else
                {
                    try
                    {
                        if (File.Exists(s))
                        {
                            FileInfo fi = new FileInfo(s);
                            if (Targets.IndexOf(Targets.Where(X => X.Path == fi.FullName).FirstOrDefault()) < 0)
                                Targets.Add(new ItemToRename { Path = fi.FullName, Before = fi.Name, After = GlobalConst.EMPTY_STRING, Result = GlobalConst.EMPTY_STRING });
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(this, "Failed to perform the" +
                          " specified operation:\n\n" + ex.Message,
                          "File operation failed");
                    }
                }
            }
            if (Targets.Count > 0)
            {
                lbl_TargetCounts.Content = "No. of Items : " + Targets.Count;
                lView_TargetList.ItemsSource = Targets;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lView_TargetList.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("Path", ListSortDirection.Ascending));
                lView_TargetList.Items.Refresh();
            }
        }

        private void lView_TargetList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void lView_TargetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lView_TargetList.SelectedItems.Count > 0)
            {
                btn_Remove_Item.IsEnabled = true;
            }
            else
            {
                btn_Remove_Item.IsEnabled = false;
            }
        }

        private void lView_TargetList_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            // Set the effect based upon the KeyState.
            if ((e.KeyStates & GlobalConst.KEY_SHIFT) == GlobalConst.KEY_SHIFT &&
                (e.AllowedEffects & DragDropEffects.Move) == DragDropEffects.Move)
            {
                e.Effects = DragDropEffects.Move;

            }
            else if ((e.KeyStates & GlobalConst.KEY_CTRL) == GlobalConst.KEY_CTRL &&
                (e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                e.Effects = DragDropEffects.Copy;
            }
            else if ((e.AllowedEffects & DragDropEffects.Move) == DragDropEffects.Move)
            {
                // By default, the drop action should be move, if allowed.
                e.Effects = DragDropEffects.Move;

                switch (cbox_TargetType.SelectedItem.ToString())
                {
                    case GlobalConst.TARGETTYPE_FOLDERNAME:
                        string[] folders = (string[])e.Data.GetData(DataFormats.FileDrop);
                        if (folders.Length > 0 && (e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
                            e.Effects = DragDropEffects.Copy;
                        break;
                    default:
                        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                        if (files.Length > 0 && (e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
                            e.Effects = DragDropEffects.Copy;
                        break;
                }
            }
            else
                e.Effects = DragDropEffects.None;

            // This is an example of how to get the item under the mouse
            //Point pt = lView_TargetList.PointToClient(new Point(e.X, e.Y));
            //ListViewItem itemUnder = lView_TargetList.GetItemAt(pt.X, pt.Y);
        }

        private void btn_Reset_Filelist_Click(object sender, RoutedEventArgs e)
        {
            Targets.Clear();
            lbl_TargetCounts.Content = "No. of Items : 0";
            lView_TargetList.ItemsSource = null;
            lView_TargetList.Items.Clear();
            lView_TargetList.Items.Refresh();
            listView_CollectionChanged(lView_Rules, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            btn_Remove_Item.IsEnabled = false;
            btn_Undo.IsEnabled = false;
        }

        private void btn_Remove_Item_Click(object sender, RoutedEventArgs e)
        {

            if (lView_TargetList.SelectedItems.Count != 0)
            {
                IEditableCollectionView items = lView_TargetList.Items;
                while (lView_TargetList.SelectedItems.Count != 0)
                {
                    if (items.CanRemove)
                    {
                        items.Remove(lView_TargetList.SelectedItems[0]);
                    }
                }
            }
            lbl_TargetCounts.Content = "No. of Items : " + Targets.Count;
        }
        #endregion


        #region lView_Rules
        private void lView_Rules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lView_Rules.SelectedItems.Count == 0)
            {
                btn_UpdateRule.IsEnabled = false;
                btn_MoveUp.IsEnabled = false;
                btn_MoveDn.IsEnabled = false;
                btn_RemoveRule.IsEnabled = false;
                return;
            }

            if (lView_Rules.SelectedItems.Count == 1)
            {
                cbox_TargetType.SelectedIndex = cbox_TargetType.Items.IndexOf(((Rule)lView_Rules.SelectedItem).Target);
                cbox_RenameMode.SelectedIndex = cbox_RenameMode.Items.IndexOf(((Rule)lView_Rules.SelectedItem).Mode);
                cbox_1stParam.Text = string.IsNullOrEmpty(((Rule)lView_Rules.SelectedItem).p1) ? GlobalConst.EMPTY_STRING : ((Rule)lView_Rules.SelectedItem).p1;
                cbox_2ndParam.Text = string.IsNullOrEmpty(((Rule)lView_Rules.SelectedItem).p2) ? GlobalConst.EMPTY_STRING : ((Rule)lView_Rules.SelectedItem).p2;
                btn_UpdateRule.IsEnabled = true;
                btn_MoveUp.IsEnabled = true;
                btn_MoveDn.IsEnabled = true;
                btn_RemoveRule.IsEnabled = true;
                if (cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_INSERT)
                {
                    switch (cbox_1stParam.Text)
                    {
                        case GlobalConst.FUNCTYPE_PHASE:
                            cbox_1stSubParam.Text = string.IsNullOrEmpty(((Rule)lView_Rules.SelectedItem).Sub) ? GlobalConst.EMPTY_STRING : ((Rule)lView_Rules.SelectedItem).Sub;
                            break;
                        case GlobalConst.FUNCTYPE_DIGIT:
                            string[] auxsplit = ((Rule)lView_Rules.SelectedItem).Sub.Split(';');
                            cbox_1stSubParam.Text = auxsplit[0];
                            cbox_2ndSubParam.Text = auxsplit[1];
                            cbox_3rdSubParam.Text = auxsplit[2];
                            break;
                    }
                }
            }
        }

        private void btn_AddRule_Click(object sender, RoutedEventArgs e)
        {

            if ((!string.IsNullOrEmpty(cbox_TargetType.SelectedItem.ToString()) &&
                !string.IsNullOrEmpty(cbox_RenameMode.SelectedItem.ToString()) &&
                !string.IsNullOrEmpty(cbox_1stParam.Text)) ||
            (cbox_TargetType.SelectedItem.ToString() == GlobalConst.TARGETTYPE_FOLDERNAME &&
            cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_MAKERAR) ||
            (cbox_TargetType.SelectedItem.ToString() == GlobalConst.TARGETTYPE_FILENAME &&
            cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_BMP2PNG))
            {
                string aux = GlobalConst.EMPTY_STRING;
                string p1, p2, sub = GlobalConst.EMPTY_STRING;
                if (!string.IsNullOrEmpty(cbox_1stParam.Text))
                {
                    p1 = cbox_1stParam.Text;
                }
                else
                {
                    p1 = GlobalConst.EMPTY_STRING;
                }
                if (!string.IsNullOrEmpty(cbox_2ndParam.Text))
                {
                    switch (cbox_1stParam.Text)
                    {
                        case GlobalConst.FUNCTYPE_LEFT:
                        case GlobalConst.FUNCTYPE_RIGHT:
                            try
                            {
                                Convert.ToInt32(cbox_2ndParam.Text);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Numbers only !");
                                cbox_2ndParam.Text = GlobalConst.ZERO;
                            }
                            break;
                        default:
                            break;
                    }
                    p2 = cbox_2ndParam.Text;
                }
                else
                {
                    p2 = GlobalConst.EMPTY_STRING;
                }
                if (cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_INSERT)
                {
                    switch (cbox_1stParam.Text)
                    {
                        case GlobalConst.FUNCTYPE_PHASE:
                            aux = cbox_1stSubParam.Text;
                            break;
                        case GlobalConst.FUNCTYPE_DIGIT:
                            if (!string.IsNullOrEmpty(cbox_1stSubParam.Text) && !string.IsNullOrEmpty(cbox_2ndSubParam.Text) && !string.IsNullOrEmpty(cbox_3rdSubParam.Text))
                                aux = string.Join(GlobalConst.AUX_CONNECTOR, new string[] { cbox_1stSubParam.Text, cbox_2ndSubParam.Text, cbox_3rdSubParam.Text });
                            break;
                    }
                    if (!string.IsNullOrEmpty(aux)) sub = aux;
                    cbox_1stSubParam.Text = GlobalConst.EMPTY_STRING;
                    cbox_2ndSubParam.Text = GlobalConst.EMPTY_STRING;
                    cbox_3rdSubParam.Text = GlobalConst.EMPTY_STRING;
                }
                rules.Add(new Rule
                {
                    Enable = true,
                    Target = cbox_TargetType.SelectedItem.ToString(),
                    Mode = cbox_RenameMode.Text,
                    p1 = p1,
                    p2 = p2,
                    Sub = sub
                });
                lView_Rules.ItemsSource = rules;
                lView_Rules.Items.Refresh();
                cbox_1stParam.Text = (cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_INSERT) ? cbox_1stParam.Text : GlobalConst.EMPTY_STRING;
                cbox_2ndParam.Text = GlobalConst.EMPTY_STRING;
            }
        }

        private void btn_UpdateRule_Click(object sender, RoutedEventArgs e)
        {
            if (lView_Rules.SelectedItems.Count == 1 &&
                !string.IsNullOrEmpty(cbox_TargetType.SelectedItem.ToString()) && !string.IsNullOrEmpty(cbox_RenameMode.SelectedItem.ToString()) &&
                !string.IsNullOrEmpty(cbox_1stParam.Text))
            {
                ((Rule)lView_Rules.SelectedItem).Target = cbox_TargetType.SelectedItem.ToString();
                ((Rule)lView_Rules.SelectedItem).Mode = cbox_RenameMode.SelectedItem.ToString();
                ((Rule)lView_Rules.SelectedItem).p1 = cbox_1stParam.Text;
                ((Rule)lView_Rules.SelectedItem).p2 = cbox_2ndParam.Text;
                btn_UpdateRule.IsEnabled = false;
                if (cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_INSERT)
                {
                    switch (cbox_1stParam.SelectedItem.ToString())
                    {
                        case GlobalConst.FUNCTYPE_PHASE:
                            ((Rule)lView_Rules.SelectedItem).Sub = cbox_1stSubParam.Text;
                            break;
                        case GlobalConst.FUNCTYPE_DIGIT:
                            if (!string.IsNullOrEmpty(cbox_1stSubParam.Text) && !string.IsNullOrEmpty(cbox_2ndSubParam.Text) && !string.IsNullOrEmpty(cbox_3rdSubParam.Text))
                                ((Rule)lView_Rules.SelectedItem).Sub = string.Join(GlobalConst.AUX_CONNECTOR, new string[] { cbox_1stSubParam.Text, cbox_2ndSubParam.Text, cbox_3rdSubParam.Text });
                            break;
                    }
                }
                lView_Rules.SelectedItem = null;
                lView_Rules.Items.Refresh();
            }
        }

        private void btn_MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (lView_Rules.SelectedItems.Count == 1)
            {
                int seletedIndex = lView_Rules.SelectedIndex;
                MoveItems(lView_Rules, MoveDirection.Up);
                if (seletedIndex > 0)
                {
                    lView_Rules.SelectedIndex = seletedIndex - 1;
                }
                else
                {
                    lView_Rules.SelectedIndex = seletedIndex;
                }
                lView_Rules.Focus(); //Select();
            }
        }

        private void btn_MoveDn_Click(object sender, RoutedEventArgs e)
        {
            if (lView_Rules.SelectedItems.Count == 1)
            {
                int seletedIndex = lView_Rules.SelectedIndex;
                MoveItems(lView_Rules, MoveDirection.Down);
                if (seletedIndex > 0 && seletedIndex < lView_Rules.Items.Count - 1)
                {
                    lView_Rules.SelectedIndex = seletedIndex + 1;
                }
                else
                {
                    lView_Rules.SelectedIndex = seletedIndex;
                }
                lView_Rules.Focus(); //Select();
            }
        }

        private void btn_ResetRule_Click(object sender, RoutedEventArgs e)
        {
            lView_Rules.ItemsSource = null;
            rules.Clear();
            lView_Rules.Items.Clear();
            lView_Rules.Items.Refresh();
            listView_CollectionChanged(lView_Rules, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            MakeRAR = false;
            BMP2PNG = false;
        }

        private void btn_RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            if (((Rule)lView_Rules.SelectedItem).Mode == GlobalConst.MODETYPE_MAKERAR) MakeRAR = false;
            if (((Rule)lView_Rules.SelectedItem).Mode == GlobalConst.MODETYPE_BMP2PNG) BMP2PNG = false;
            IEditableCollectionView items = lView_Rules.Items; //Cast to interface
            if (items.CanRemove)
            {
                items.Remove(lView_Rules.SelectedItem);
            }
            if (lView_Rules.Items.Count == 0)
            {
                listView_CollectionChanged(lView_Rules, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void btn_SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            if (lView_Rules.Items.Count > 0)
            {
                List<Rule> Itemlist = new List<Rule>();
                foreach (Rule vi in lView_Rules.Items)
                {
                    Itemlist.Add(vi);
                }
                if (Itemlist.Any())
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog()
                    {
                        Filter = GlobalConst.FILETYPE_XML_FILTER
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        xmlH.SaveProfileXML(Itemlist, saveFileDialog.FileName);
                    }
                }
            }
        }

        private void btn_LoadProfile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = GlobalConst.FILETYPE_XML_FILTER,
                FilterIndex = 0,
                DefaultExt = GlobalConst.FILETYPE_XML
            };
            if (ofd.ShowDialog() == true)
            {
                if (!String.Equals(System.IO.Path.GetExtension(ofd.FileName),
                                   "." + GlobalConst.FILETYPE_XML,
                                   StringComparison.OrdinalIgnoreCase))
                {
                    // Invalid file type selected; display an error.
                    MessageBox.Show("The type of the selected file is not supported by this application. You must select an " + GlobalConst.FILETYPE_XML + " file.",
                                    "Invalid File Type");
                }
                else
                {
                    lView_Rules.ItemsSource = null;
                    lView_Rules.Items.Clear();
                    lView_Rules.Items.Refresh();
                    rules = xmlH.LoadProfileXML(ofd.FileName);
                    if (rules.Count > 0) lView_Rules.ItemsSource = rules;
                }
            }
        }
        private void listView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //The projects ListView has been changed
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    //MessageBox.Show("An Item Has Been Added To The ListView!");
                    if (lView_TargetList.Items.Count > 0)
                    {
                        btn_Preview.IsEnabled = true;
                        btn_GO.IsEnabled = true;
                    }
                    if (lView_Rules.Items.Count > 0)
                    {
                        btn_SaveProfile.IsEnabled = true;
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    //MessageBox.Show("The ListView Has Been Cleared!");
                    if (lView_TargetList.Items.Count == 0)
                    {
                        btn_Preview.IsEnabled = false;
                        btn_GO.IsEnabled = false;
                        btn_Reset_Filelist.IsEnabled = false;
                    }
                    else
                    {
                        btn_Preview.IsEnabled = true;
                        btn_GO.IsEnabled = true;
                        btn_Reset_Filelist.IsEnabled = true;

                    }
                    if (lView_Rules.Items.Count == 0)
                    {
                        btn_SaveProfile.IsEnabled = false;
                        btn_ResetRule.IsEnabled = false;
                    }
                    else
                    {
                        btn_SaveProfile.IsEnabled = true;
                        btn_ResetRule.IsEnabled = true;
                    }
                    break;
            }
        }
        private void MoveItems(ListView sender, MoveDirection direction)
        {
            Rule selectedfile = lView_Rules.SelectedItem as Rule;
            int index = rules.IndexOf(selectedfile);
            bool valid = sender.SelectedItems.Count > 0 &&
                        ((direction == MoveDirection.Down && (index < sender.Items.Count - 1))
                        || (direction == MoveDirection.Up && (sender.SelectedIndex > 0)));

            if (valid)
            {
                if (direction == MoveDirection.Up)
                {
                    if (index > 0)
                    {
                        rules.Remove(selectedfile);
                        rules.Insert(index - 1, selectedfile);
                    }
                }
                else
                {
                    if (index < rules.Count - 1)
                    {
                        rules.Remove(selectedfile);
                        rules.Insert(index + 1, selectedfile);
                    }
                }
                lView_Rules.ItemsSource = null;
                lView_Rules.ItemsSource = rules;
                lView_Rules.Items.Refresh();
                lView_Rules.SelectedItem = selectedfile;
            }
        }
        #endregion

        #region ComboBoxes
        private void cbox_TargetType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbl_2ndParam.Content = GlobalConst.LBL_PARAMETER2_DEFAULT;
            AuxParameters_Control(false);
            cbox_RenameMode_init(false);
        }

        private void cbox_RenameMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbox_1stParam.ItemsSource = null;
            cbox_1stParam.Items.Clear();
            cbox_1stParam.Items.Refresh();
            lbl_2ndParam.Content = GlobalConst.LBL_PARAMETER2_DEFAULT;
            AuxParameters_Control(false);
            List<string> cbox_1st_Functions = new List<string>();
            if (cbox_RenameMode.SelectedItem != null)
                switch (cbox_RenameMode.SelectedItem.ToString())
                {
                    case GlobalConst.MODETYPE_PREFIX:
                    case GlobalConst.MODETYPE_SUFFIX:
                        lbl_1stParam.Content = GlobalConst.LBL_PARAMETER1_TARGETPHASE;
                        cbox_1stParam.Text = GlobalConst.EMPTY_STRING;
                        cbox_2ndParam_Controls(false, null);
                        break;
                    case GlobalConst.MODETYPE_REPLACE:
                        lbl_1stParam.Content = GlobalConst.LBL_PARAMETER1_TARGETPHASE;
                        cbox_1stParam.Text = GlobalConst.EMPTY_STRING;
                        cbox_2ndParam_Controls(true, GlobalConst.LBL_PARAMETER2_REPLACEWITH);
                        break;
                    case GlobalConst.MODETYPE_REMOVE:
                    case GlobalConst.MODETYPE_UPPERCASE:
                    case GlobalConst.MODETYPE_LOWERCASE:
                        lbl_1stParam.Content = GlobalConst.LBL_PARAMETER1_PATTERN;
                        cbox_1stParam.Text = GlobalConst.EMPTY_STRING;
                        cbox_2ndParam_Controls(false, null);
                        cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_ALL);
                        cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_LEFT);
                        cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_RIGHT);
                        if (cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_REMOVE)
                        {
                            cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_CENTER);
                        }
                        cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_FIRSTLETTER);
                        cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_MATCHPHASE);
                        if (cbox_RenameMode.SelectedItem.ToString() == GlobalConst.MODETYPE_REMOVE)
                        {
                            cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_BETWEENBRACKETS);
                            cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_FROMLEFTBRACKET);
                            cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_BEFORERIGHTBRACKET);
                        }
                        break;
                    case GlobalConst.MODETYPE_INSERT:
                        lbl_1stParam.Content = GlobalConst.LBL_PARAMETER1_PATTERN;
                        cbox_1stParam.Text = GlobalConst.EMPTY_STRING;
                        cbox_2ndParam_Controls(false, null);
                        cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_PHASE);
                        cbox_1st_Functions.Add(GlobalConst.FUNCTYPE_DIGIT);
                        break;
                    case GlobalConst.MODETYPE_MAKERAR:
                        lbl_1stParam.Content = GlobalConst.LBL_PARAMETER1_DEFAULT;
                        cbox_1stParam.Text = GlobalConst.EMPTY_STRING;
                        cbox_2ndParam_Controls(false, null);
                        break;
                }
            cbox_1stParam.Items.Clear();
            cbox_1stParam.ItemsSource = cbox_1st_Functions;
            cbox_1stParam.Items.Refresh();
        }

        private void cbox_1stParam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbox_RenameMode.Text)
            {
                case GlobalConst.MODETYPE_REMOVE:
                case GlobalConst.MODETYPE_UPPERCASE:
                case GlobalConst.MODETYPE_LOWERCASE:
                    if (cbox_1stParam.SelectedItem != null)
                        switch (cbox_1stParam.SelectedItem.ToString())
                        {
                            case GlobalConst.FUNCTYPE_BETWEENBRACKETS:
                            case GlobalConst.FUNCTYPE_FROMLEFTBRACKET:
                            case GlobalConst.FUNCTYPE_BEFORERIGHTBRACKET:
                            case GlobalConst.FUNCTYPE_FIRSTLETTER:
                            case GlobalConst.FUNCTYPE_ALL:
                                cbox_2ndParam_Controls(false, null);
                                break;
                            case GlobalConst.FUNCTYPE_LEFT:
                            case GlobalConst.FUNCTYPE_RIGHT:
                                cbox_2ndParam_Controls(true, GlobalConst.LBL_PARAMETER2_LENGTH);
                                break;
                            case GlobalConst.FUNCTYPE_CENTER:
                                cbox_2ndParam_Controls(true, GlobalConst.LBL_PARAMETER2_RANGE);
                                break;
                            case GlobalConst.FUNCTYPE_MATCHPHASE:
                                cbox_2ndParam_Controls(true, GlobalConst.LBL_PARAMETER1_TARGETPHASE);
                                break;
                            default:
                                cbox_2ndParam_Controls(false, null);
                                break;
                        }
                    break;
                case GlobalConst.MODETYPE_INSERT:
                    cbox_2ndParam_Controls(true, GlobalConst.LBL_PARAMETER2_POSITION);
                    if (cbox_1stParam.SelectedItem != null)
                        switch (cbox_1stParam.SelectedItem.ToString())
                        {
                            case GlobalConst.FUNCTYPE_PHASE:
                                AuxParameters_Control(true, GlobalConst.AUX_PARAMETER_PHASE);
                                break;
                            case GlobalConst.FUNCTYPE_DIGIT:
                                AuxParameters_Control(true, GlobalConst.AUX_PARAMETER_LENGTH, GlobalConst.AUX_PARAMETER_STARTFROM, GlobalConst.AUX_PARAMETER_STEP);
                                break;
                        }
                    break;
            }
        }

        private void cbox_2ndParam_Controls(bool Visibility, string label)
        {
            cbox_2ndParam.IsEnabled = Visibility;
            lbl_2ndParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            lbl_2ndParam.Content = !string.IsNullOrEmpty(label) ? label : GlobalConst.EMPTY_STRING;
            cbox_2ndParam.Text = (lbl_2ndParam.Name == GlobalConst.LBL_PARAMETER2_POSITION || lbl_2ndParam.Name == GlobalConst.LBL_PARAMETER2_LENGTH) ? GlobalConst.ZERO : GlobalConst.EMPTY_STRING;
            cbox_2ndParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
        private void AuxParameters_Control(bool Visibility, string Ax1 = null, string Ax2 = null, string Ax3 = null)
        {
            lbl_1stSubParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            lbl_1stSubParam.Content = !string.IsNullOrEmpty(Ax1) ? Ax1 : GlobalConst.EMPTY_STRING;
            cbox_1stSubParam.Text = GlobalConst.EMPTY_STRING;
            cbox_1stSubParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            cbox_1stSubParam.IsEnabled = Visibility;

            if (string.IsNullOrEmpty(Ax2))
            {
                Visibility = false;
            }
            lbl_2ndSubParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            lbl_2ndSubParam.Content = !string.IsNullOrEmpty(Ax2) ? Ax2 : GlobalConst.EMPTY_STRING;
            cbox_2ndSubParam.Text = GlobalConst.EMPTY_STRING;
            cbox_2ndSubParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            cbox_2ndSubParam.IsEnabled = Visibility;

            if (string.IsNullOrEmpty(Ax3))
            {
                Visibility = false;
            }
            lbl_3rdSubParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            lbl_3rdSubParam.Content = !string.IsNullOrEmpty(Ax3) ? Ax3 : GlobalConst.EMPTY_STRING;
            cbox_3rdSubParam.Text = GlobalConst.EMPTY_STRING;
            cbox_3rdSubParam.Visibility = Visibility ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            cbox_3rdSubParam.IsEnabled = Visibility;
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            btn_Undo.IsEnabled = false;
            btn_UpdateRule.IsEnabled = false;
            btn_MoveUp.IsEnabled = false;
            btn_MoveDn.IsEnabled = false;
            btn_RemoveRule.IsEnabled = false;
            btn_Remove_Item.IsEnabled = false;
            btn_Preview.IsEnabled = false;
            btn_GO.IsEnabled = false;
            btn_SaveProfile.IsEnabled = false;
            btn_ResetRule.IsEnabled = false;
            btn_Reset_Filelist.IsEnabled = false;
            AuxParameters_Control(false);
            cbox_TargetType_init();
            ((INotifyCollectionChanged)lView_Rules.Items).CollectionChanged += listView_CollectionChanged;
            ((INotifyCollectionChanged)lView_TargetList.Items).CollectionChanged += listView_CollectionChanged;
        }
        private void cbox_TargetType_init()
        {
            List<string> Targettypes = new List<string>
            {
                GlobalConst.TARGETTYPE_FILENAME,
                GlobalConst.TARGETTYPE_FILEEXTENSION
            };
            if (File.Exists(GlobalConst.PATH_WINRAR)) Targettypes.Add(GlobalConst.TARGETTYPE_FOLDERNAME);
            cbox_TargetType.ItemsSource = null;
            cbox_TargetType.ItemsSource = Targettypes;
            cbox_TargetType.Items.Refresh();
            cbox_TargetType.SelectedIndex = 0;
            cbox_RenameMode_init(true);
        }
        private void cbox_RenameMode_init(bool init)
        {
            List<string> Modes = new List<string>
            {
                GlobalConst.MODETYPE_PREFIX,
                GlobalConst.MODETYPE_SUFFIX,
                GlobalConst.MODETYPE_REMOVE,
                GlobalConst.MODETYPE_REPLACE,
                GlobalConst.MODETYPE_UPPERCASE,
                GlobalConst.MODETYPE_LOWERCASE,
                GlobalConst.MODETYPE_INSERT
            };
            if (cbox_TargetType.SelectedItem.ToString() == GlobalConst.TARGETTYPE_FILENAME) Modes.Add(GlobalConst.MODETYPE_BMP2PNG);
            if (cbox_TargetType.SelectedItem.ToString() == GlobalConst.TARGETTYPE_FOLDERNAME) Modes.Add(GlobalConst.MODETYPE_MAKERAR);
            cbox_RenameMode.Visibility = System.Windows.Visibility.Visible;
            lbl_RenameMode.Visibility = System.Windows.Visibility.Visible;
            cbox_1stParam.Visibility = System.Windows.Visibility.Visible;
            lbl_1stParam.Visibility = System.Windows.Visibility.Visible;
            cbox_2ndParam.Visibility = System.Windows.Visibility.Visible;
            lbl_2ndParam.Visibility = System.Windows.Visibility.Visible;
            cbox_RenameMode.ItemsSource = null;
            cbox_RenameMode.ItemsSource = Modes;
            cbox_RenameMode.Items.Refresh();
            if (init) cbox_RenameMode.SelectedIndex = 3;
        }
    }
}

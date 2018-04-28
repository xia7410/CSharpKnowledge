﻿using System;
using System.Collections.Generic;
using System.Data;
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
using WPF.ViewModel;
using System.Threading;

namespace WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑;
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Person> persons = new List<Person>();

        public DataGridVM m_DgVm;            // DataGrid的VM层，所有针对DataGrid的操作，全部使用这个类型;

        public MainWindow()
        {
            InitializeComponent();
            //DrawPoin();
            DrawPolyLine();                 // WPF画线;
            InitPersons();                  // 初始化Person列表;
            InitStus();                     // 使用LINQ筛选;

            //_________________________以下是初始化一个DataGrid的几列数据__________
            List<string> colums = new List<string>();
            colums.Add("column1");
            colums.Add("column2");
            colums.Add("c3");
            TryAddDataGridColumn(colums);


            //_________________________以下是使用Binding关联XAML和C#代码的实验_____

            // 这个是ItemControl类的控件使用binding关联数据源的方法;
            this.PersonList.ItemsSource = persons;
            this.PersonList.DisplayMemberPath = "m_Name";
            this.PersonList.SelectionChanged += PersonList_SelectionChanged;
            
            // 通过SetBinding方法也可以Binding现有类(ListBox)的属性;
            this.PersonID.SetBinding(TextBox.TextProperty, new Binding("m_ID") {
                Source = this.PersonList.ItemsSource,
                BindsDirectlyToSource = true
            });
            
            // 以下是通过ObjectDataProvider进行命令的binding
            ObjectDataProvider odp = new ObjectDataProvider();
            odp.ObjectInstance = new Calc();
            odp.MethodName = "Add";
            odp.MethodParameters.Add("0");
            odp.MethodParameters.Add("0");

            // 方法的第一个参数;
            this.CalcX.SetBinding(TextBox.TextProperty, new Binding("MethodParameters[0]") {
                Source = odp,
                BindsDirectlyToSource = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            // 方法的第二个参数;
            this.CalcY.SetBinding(TextBox.TextProperty, new Binding("MethodParameters[1]")
            {
                Source = odp,
                BindsDirectlyToSource = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            this.CalcRes.SetBinding(TextBox.TextProperty, new Binding(".")
            {
                Source = odp
            });

        }

        /// <summary>
        /// 初始化列;
        /// </summary>
        /// <param name="list">初始化列的string列表</param>
        private void TryAddDataGridColumn(List<string> list)
        {
            m_DgVm = new DataGridVM(list);
            m_DgVm.m_ColumnNameList.ListChanged += M_ColumnNameList_ListChanged;
            
            foreach (string iter in m_DgVm.m_ColumnNameList.m_list)
            {
                MessageDataGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = iter
                });
            }
        }

        /// <summary>
        /// DataGridVM层列表发生变化时,将变化添加到View中;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_ColumnNameList_ListChanged(object sender, EventArgs e)
        {
            MessageDataGrid.Columns.Clear();

            foreach (string iter in m_DgVm.m_ColumnNameList.m_list)
            {
                MessageDataGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = iter,
                    Width = iter.Length * 10
                });
            }
        }

        private void PersonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.PersonID.Text = (this.PersonList.SelectedItem as Person).m_ID.ToString();
        }

        private void InitStus()
        {
            // 使用LINQ查询;
            this.StuList.ItemsSource = from stu in this.stus.m_StuList where stu.m_Name.StartsWith("G") select stu;
        }

        private void InitPersons()
        {
            persons.Add(new Person(11, "Guoliang"));
            persons.Add(new Person(2, "Roupao"));
            persons.Add(new Person(5, "WangCY"));
        }

        public void DrawPoin()
        {
            Canvas myParentCanvas;
            Canvas myCanvas1;
            Canvas myCanvas2;
            Canvas myCanvas3;

            myParentCanvas = new Canvas();
            myParentCanvas.Width = 400;
            myParentCanvas.Height = 400;

            // Define child Canvas elements
            myCanvas1 = new Canvas();
            myCanvas1.Background = Brushes.Red;
            myCanvas1.Height = 100;
            myCanvas1.Width = 100;
            Canvas.SetTop(myCanvas1, 0);
            Canvas.SetLeft(myCanvas1, 0);

            myCanvas2 = new Canvas();
            myCanvas2.Background = Brushes.Green;
            myCanvas2.Height = 100;
            myCanvas2.Width = 100;
            Canvas.SetTop(myCanvas2, 100);
            Canvas.SetLeft(myCanvas2, 100);

            myCanvas3 = new Canvas();
            myCanvas3.Background = Brushes.Blue;
            myCanvas3.Height = 50;
            myCanvas3.Width = 50;
            Canvas.SetTop(myCanvas3, 50);
            Canvas.SetLeft(myCanvas3, 50);

            // Add child elements to the Canvas' Children collection
            myParentCanvas.Children.Add(myCanvas1);
            myParentCanvas.Children.Add(myCanvas2);
            myParentCanvas.Children.Add(myCanvas3);

            // Add the parent Canvas as the Content of the Window Object
            this.Content = myParentCanvas;
        }

        public void DrawPolyLine()
        {
            Polyline LineSeries = new Polyline();
            LineSeries.Points.Add(new Point(0, 11));
            LineSeries.Points.Add(new Point(10, 21));
            LineSeries.Points.Add(new Point(10, 31));
            LineSeries.Points.Add(new Point(120, 31));

            LineSeries.Stroke = Brushes.Blue;      // 颜色;
            LineSeries.StrokeThickness = 13;       // 粗细;

            this.canvas1.Children.Add(LineSeries);
        }

        // 实验一下添加列;
        private void ChangeCol_Click(object sender, RoutedEventArgs e)
        {
            this.m_DgVm.m_ColumnNameList.Add("CCCCCCCCCCC3");
            List<string> temp = new List<string>();
            temp.Add("A111");
            temp.Add("A222");
            temp.Add("A333");
            this.m_DgVm.m_ColumnNameList.CopyFrom(temp);
        }
    }
}

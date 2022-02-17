﻿/******************************************************************************
 * SunnyUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2012-2021 ShenYongHua(沈永华).
 * QQ群：56829229 QQ：17612584 EMail：SunnyUI@QQ.Com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: UIComboBox.cs
 * 文件说明: 组合框
 * 当前版本: V3.0
 * 创建日期: 2020-01-01
 *
 * 2020-01-01: V2.2.0 增加文件说明
 * 2020-06-11: V2.2.5 增加DataSource，支持数据绑定
 * 2021-05-06: V3.0.3 解决鼠标下拉选择，触发SelectedIndexChanged两次的问题
 * 2021-06-03: V3.0.4 更新了数据绑定相关代码
 * 2021-08-03: V3.0.5 Items.Clear后清除显示
 * 2021-08-15: V3.0.6 重写了水印文字的画法，并增加水印文字颜色
******************************************************************************/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Sunny.UI
{
    [DefaultProperty("Items")]
    [DefaultEvent("SelectedIndexChanged")]
    [ToolboxItem(true)]
    [LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
    public sealed partial class UIComboBox : UIDropControl, IToolTip
    {
        public UIComboBox()
        {
            InitializeComponent();
            ListBox.SelectedIndexChanged += Box_SelectedIndexChanged;
            ListBox.DataSourceChanged += Box_DataSourceChanged;
            ListBox.DisplayMemberChanged += Box_DisplayMemberChanged;
            ListBox.ValueMemberChanged += Box_ValueMemberChanged;
            ListBox.SelectedValueChanged += ListBox_SelectedValueChanged;
            ListBox.ItemsClear += ListBox_ItemsClear;
            ListBox.ItemsRemove += ListBox_ItemsRemove;
            edit.TextChanged += Edit_TextChanged;
            DropDownWidth = 150;
            fullControlSelect = true;
        }

        public Control ExToolTipControl()
        {
            return edit;
        }

        [DefaultValue(false)]
        public bool Sorted
        {
            get => ListBox.Sorted;
            set => ListBox.Sorted = value;
        }

        public int FindString(string s)
        {
            return ListBox.FindString(s);
        }

        public int FindString(string s, int startIndex)
        {
            return ListBox.FindString(s, startIndex);
        }

        public int FindStringExact(string s)
        {
            return ListBox.FindStringExact(s);
        }

        public int FindStringExact(string s, int startIndex)
        {
            return ListBox.FindStringExact(s, startIndex);
        }

        private void ListBox_ItemsRemove(object sender, EventArgs e)
        {
            if (ListBox.Count == 0)
            {
                Text = "";
                edit.Text = "";
            }
        }

        private void ListBox_ItemsClear(object sender, EventArgs e)
        {
            Text = "";
            edit.Text = "";
        }

        public new event EventHandler TextChanged;

        private void Edit_TextChanged(object sender, EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        private void ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedValueChanged?.Invoke(this, e);
        }

        private void Box_ValueMemberChanged(object sender, EventArgs e)
        {
            ValueMemberChanged?.Invoke(this, e);
        }

        private void Box_DisplayMemberChanged(object sender, EventArgs e)
        {
            DisplayMemberChanged?.Invoke(this, e);
        }

        private void Box_DataSourceChanged(object sender, EventArgs e)
        {
            DataSourceChanged?.Invoke(this, e);
        }

        private void Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = ListBox.GetItemText(ListBox.SelectedItem);
            SelectedIndexChanged?.Invoke(this, e);
        }

        public event EventHandler SelectedIndexChanged;

        public event EventHandler DataSourceChanged;

        public event EventHandler DisplayMemberChanged;

        public event EventHandler ValueMemberChanged;

        public event EventHandler SelectedValueChanged;

        protected override void ItemForm_ValueChanged(object sender, object value)
        {
            //if (SelectedIndex != ListBox.SelectedIndex)
            //{
            //    SelectedIndex = ListBox.SelectedIndex;
            //    //Box_SelectedIndexChanged(null, null);
            //    Invalidate();
            //}

            Invalidate();
        }

        private readonly UIComboBoxItem dropForm = new UIComboBoxItem();

        protected override void CreateInstance()
        {
            ItemForm = new UIDropDown(dropForm);
        }

        protected override int CalcItemFormHeight()
        {
            int interval = ItemForm.Height - ItemForm.ClientRectangle.Height;
            return 4 + Math.Min(ListBox.Items.Count, MaxDropDownItems) * ItemHeight + interval;
        }

        private UIListBox ListBox
        {
            get => dropForm.ListBox;
        }

        [DefaultValue(25)]
        [Description("列表项高度"), Category("SunnyUI")]
        public int ItemHeight
        {
            get => ListBox.ItemHeight;
            set => ListBox.ItemHeight = value;
        }

        [DefaultValue(8)]
        [Description("列表下拉最大个数"), Category("SunnyUI")]
        public int MaxDropDownItems { get; set; } = 8;

        private void UIComboBox_FontChanged(object sender, EventArgs e)
        {
            if (ItemForm != null)
            {
                ListBox.Font = Font;
            }
        }

        public void ShowDropDown()
        {
            UIComboBox_ButtonClick(this, EventArgs.Empty);
        }

        private void UIComboBox_ButtonClick(object sender, EventArgs e)
        {
            if (Items.Count > 0)
                ItemForm.Show(this, new Size(DropDownWidth < Width ? Width : DropDownWidth, CalcItemFormHeight()));
        }

        public override void SetStyleColor(UIBaseStyle uiColor)
        {
            base.SetStyleColor(uiColor);
            ListBox.SetStyleColor(uiColor);
        }

        public object DataSource
        {
            get => ListBox.DataSource;
            set => ListBox.DataSource = value;
        }

        [DefaultValue(150)]
        [Description("下拉框宽度"), Category("SunnyUI")]
        public int DropDownWidth { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [MergableProperty(false)]
        [Description("列表项"), Category("SunnyUI")]
        public ListBox.ObjectCollection Items => ListBox.Items;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("选中索引"), Category("SunnyUI")]
        public int SelectedIndex
        {
            get => ListBox.SelectedIndex;
            set => ListBox.SelectedIndex = value;
        }

        [Browsable(false), Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("选中项"), Category("SunnyUI")]
        public object SelectedItem
        {
            get => ListBox.SelectedItem;
            set => ListBox.SelectedItem = value;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("选中文字"), Category("SunnyUI")]
        public string SelectedText
        {
            get
            {
                if (DropDownStyle == UIDropDownStyle.DropDown)
                {
                    return edit.SelectedText;
                }
                else
                {
                    return Text;
                }
            }
        }

        public override void ResetText()
        {
            Clear();
        }

        [Description("获取或设置要为此列表框显示的属性。"), Category("SunnyUI")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string DisplayMember
        {
            get => ListBox.DisplayMember;
            set => ListBox.DisplayMember = value;
        }

        [Description("获取或设置指示显示值的方式的格式说明符字符。"), Category("SunnyUI")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [MergableProperty(false)]
        public string FormatString
        {
            get => ListBox.FormatString;
            set => ListBox.FormatString = value;
        }

        [Description("获取或设置指示显示值是否可以进行格式化操作。"), Category("SunnyUI")]
        [DefaultValue(false)]
        public bool FormattingEnabled
        {
            get => ListBox.FormattingEnabled;
            set => ListBox.FormattingEnabled = value;
        }

        [Description("获取或设置要为此列表框实际值的属性。"), Category("SunnyUI")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string ValueMember
        {
            get => ListBox.ValueMember;
            set => ListBox.ValueMember = value;
        }

        [
            DefaultValue(null),
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
            Bindable(true)
        ]
        public object SelectedValue
        {
            get => ListBox.SelectedValue;
            set => ListBox.SelectedValue = value;
        }

        public string GetItemText(object item)
        {
            return ListBox.GetItemText(item);
        }

        private void UIComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowDropDown();
            }
        }

        private void edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowDropDown();
            }
        }
    }
}
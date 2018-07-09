Public Class Form1
    Inherits System.Windows.Forms.Form
    Dim hComm As Integer                'COMポート用ハンドリング
    Dim stDCB As DCB                    'DCB構造体定義
    Dim timeOut As COMMTIMEOUTS         'COM用タイムアウト定数
    Dim wDATA As String                 'USB送信バッファ
    Dim rDATA(100) As Byte              'USB受信バッファ
    Dim dLen, wLen, rLen As Int32       'USB送受信用パラメータ
    Dim bRet As Boolean                 'USB関数戻り値
    Dim CommName As String              'COMポート番号

#Region " Windows フォーム デザイナで生成されたコード "

    Public Sub New()
        MyBase.New()

        ' この呼び出しは Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後に初期化を追加します。

    End Sub

    ' Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    ' メモ : 以下のプロシージャは、Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使って変更してください。  
    ' コード エディタを使って変更しないでください。
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Period As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Interval As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Min As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ChangeInterval As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Pattern0 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern1 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern2 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern3 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern4 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern5 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern6 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern7 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern8 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern9 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern10 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern11 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern12 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern13 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern14 As System.Windows.Forms.TextBox
    Friend WithEvents Pattern15 As System.Windows.Forms.TextBox
    Friend WithEvents Setup As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Max As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Period = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Interval = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Min = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ChangeInterval = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Pattern0 = New System.Windows.Forms.TextBox
        Me.Pattern1 = New System.Windows.Forms.TextBox
        Me.Pattern2 = New System.Windows.Forms.TextBox
        Me.Pattern3 = New System.Windows.Forms.TextBox
        Me.Pattern4 = New System.Windows.Forms.TextBox
        Me.Pattern5 = New System.Windows.Forms.TextBox
        Me.Pattern6 = New System.Windows.Forms.TextBox
        Me.Pattern7 = New System.Windows.Forms.TextBox
        Me.Pattern8 = New System.Windows.Forms.TextBox
        Me.Pattern9 = New System.Windows.Forms.TextBox
        Me.Pattern10 = New System.Windows.Forms.TextBox
        Me.Pattern11 = New System.Windows.Forms.TextBox
        Me.Pattern12 = New System.Windows.Forms.TextBox
        Me.Pattern13 = New System.Windows.Forms.TextBox
        Me.Pattern14 = New System.Windows.Forms.TextBox
        Me.Pattern15 = New System.Windows.Forms.TextBox
        Me.Setup = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Max = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(128, Byte), CType(255, Byte), CType(128, Byte))
        Me.Button1.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(16, 88)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(96, 32)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "通信開始"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(32, 48)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 26)
        Me.TextBox1.TabIndex = 1
        Me.TextBox1.Text = "COM7"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(32, 160)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(80, 26)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = "False"
        '
        'Period
        '
        Me.Period.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Period.Location = New System.Drawing.Point(240, 16)
        Me.Period.Name = "Period"
        Me.Period.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Period.Size = New System.Drawing.Size(56, 26)
        Me.Period.TabIndex = 3
        Me.Period.Text = "1023"
        Me.Period.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(136, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "PWM分解能"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(312, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 24)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "変化周期"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Interval
        '
        Me.Interval.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Interval.Location = New System.Drawing.Point(408, 16)
        Me.Interval.Name = "Interval"
        Me.Interval.Size = New System.Drawing.Size(56, 26)
        Me.Interval.TabIndex = 6
        Me.Interval.Text = "1"
        Me.Interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.Location = New System.Drawing.Point(312, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 24)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "最低照度"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Min
        '
        Me.Min.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Min.Location = New System.Drawing.Point(408, 72)
        Me.Min.Name = "Min"
        Me.Min.Size = New System.Drawing.Size(56, 26)
        Me.Min.TabIndex = 12
        Me.Min.Text = "1"
        Me.Min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.Location = New System.Drawing.Point(144, 128)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(88, 24)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "色相周期"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ChangeInterval
        '
        Me.ChangeInterval.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ChangeInterval.Location = New System.Drawing.Point(240, 128)
        Me.ChangeInterval.Name = "ChangeInterval"
        Me.ChangeInterval.Size = New System.Drawing.Size(56, 26)
        Me.ChangeInterval.TabIndex = 14
        Me.ChangeInterval.Text = "2047"
        Me.ChangeInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label7.Location = New System.Drawing.Point(144, 184)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(56, 24)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "色相"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Pattern0
        '
        Me.Pattern0.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern0.Location = New System.Drawing.Point(216, 184)
        Me.Pattern0.Name = "Pattern0"
        Me.Pattern0.Size = New System.Drawing.Size(24, 26)
        Me.Pattern0.TabIndex = 16
        Me.Pattern0.Text = "09"
        Me.Pattern0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern1
        '
        Me.Pattern1.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern1.Location = New System.Drawing.Point(248, 184)
        Me.Pattern1.Name = "Pattern1"
        Me.Pattern1.Size = New System.Drawing.Size(24, 26)
        Me.Pattern1.TabIndex = 17
        Me.Pattern1.Text = "12"
        Me.Pattern1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern2
        '
        Me.Pattern2.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern2.Location = New System.Drawing.Point(280, 184)
        Me.Pattern2.Name = "Pattern2"
        Me.Pattern2.Size = New System.Drawing.Size(24, 26)
        Me.Pattern2.TabIndex = 18
        Me.Pattern2.Text = "24"
        Me.Pattern2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern3
        '
        Me.Pattern3.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern3.Location = New System.Drawing.Point(312, 184)
        Me.Pattern3.Name = "Pattern3"
        Me.Pattern3.Size = New System.Drawing.Size(24, 26)
        Me.Pattern3.TabIndex = 19
        Me.Pattern3.Text = "1B"
        Me.Pattern3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern4
        '
        Me.Pattern4.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern4.Location = New System.Drawing.Point(344, 184)
        Me.Pattern4.Name = "Pattern4"
        Me.Pattern4.Size = New System.Drawing.Size(24, 26)
        Me.Pattern4.TabIndex = 20
        Me.Pattern4.Text = "1E"
        Me.Pattern4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern5
        '
        Me.Pattern5.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern5.Location = New System.Drawing.Point(376, 184)
        Me.Pattern5.Name = "Pattern5"
        Me.Pattern5.Size = New System.Drawing.Size(24, 26)
        Me.Pattern5.TabIndex = 21
        Me.Pattern5.Text = "36"
        Me.Pattern5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern6
        '
        Me.Pattern6.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern6.Location = New System.Drawing.Point(408, 184)
        Me.Pattern6.Name = "Pattern6"
        Me.Pattern6.Size = New System.Drawing.Size(24, 26)
        Me.Pattern6.TabIndex = 22
        Me.Pattern6.Text = "2D"
        '
        'Pattern7
        '
        Me.Pattern7.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern7.Location = New System.Drawing.Point(440, 184)
        Me.Pattern7.Name = "Pattern7"
        Me.Pattern7.Size = New System.Drawing.Size(24, 26)
        Me.Pattern7.TabIndex = 23
        Me.Pattern7.Text = "3F"
        Me.Pattern7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern8
        '
        Me.Pattern8.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern8.Location = New System.Drawing.Point(216, 216)
        Me.Pattern8.Name = "Pattern8"
        Me.Pattern8.Size = New System.Drawing.Size(24, 26)
        Me.Pattern8.TabIndex = 24
        Me.Pattern8.Text = "21"
        Me.Pattern8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern9
        '
        Me.Pattern9.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern9.Location = New System.Drawing.Point(248, 216)
        Me.Pattern9.Name = "Pattern9"
        Me.Pattern9.Size = New System.Drawing.Size(24, 26)
        Me.Pattern9.TabIndex = 25
        Me.Pattern9.Text = "14"
        Me.Pattern9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern10
        '
        Me.Pattern10.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern10.Location = New System.Drawing.Point(280, 216)
        Me.Pattern10.Name = "Pattern10"
        Me.Pattern10.Size = New System.Drawing.Size(24, 26)
        Me.Pattern10.TabIndex = 26
        Me.Pattern10.Text = "0C"
        Me.Pattern10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern11
        '
        Me.Pattern11.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern11.Location = New System.Drawing.Point(312, 216)
        Me.Pattern11.Name = "Pattern11"
        Me.Pattern11.Size = New System.Drawing.Size(24, 26)
        Me.Pattern11.TabIndex = 27
        Me.Pattern11.Text = "33"
        Me.Pattern11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern12
        '
        Me.Pattern12.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern12.Location = New System.Drawing.Point(344, 216)
        Me.Pattern12.Name = "Pattern12"
        Me.Pattern12.Size = New System.Drawing.Size(24, 26)
        Me.Pattern12.TabIndex = 28
        Me.Pattern12.Text = "00"
        Me.Pattern12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern13
        '
        Me.Pattern13.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern13.Location = New System.Drawing.Point(376, 216)
        Me.Pattern13.Name = "Pattern13"
        Me.Pattern13.Size = New System.Drawing.Size(24, 26)
        Me.Pattern13.TabIndex = 29
        Me.Pattern13.Text = "00"
        Me.Pattern13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern14
        '
        Me.Pattern14.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern14.Location = New System.Drawing.Point(408, 216)
        Me.Pattern14.Name = "Pattern14"
        Me.Pattern14.Size = New System.Drawing.Size(24, 26)
        Me.Pattern14.TabIndex = 30
        Me.Pattern14.Text = "00"
        Me.Pattern14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Pattern15
        '
        Me.Pattern15.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Pattern15.Location = New System.Drawing.Point(440, 216)
        Me.Pattern15.Name = "Pattern15"
        Me.Pattern15.Size = New System.Drawing.Size(24, 26)
        Me.Pattern15.TabIndex = 31
        Me.Pattern15.Text = "00"
        Me.Pattern15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Setup
        '
        Me.Setup.BackColor = System.Drawing.Color.FromArgb(CType(128, Byte), CType(128, Byte), CType(255, Byte))
        Me.Setup.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Setup.Location = New System.Drawing.Point(344, 288)
        Me.Setup.Name = "Setup"
        Me.Setup.Size = New System.Drawing.Size(120, 32)
        Me.Setup.TabIndex = 32
        Me.Setup.Text = "新規設定"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(224, Byte), CType(224, Byte), CType(224, Byte))
        Me.Button2.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(32, 288)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(96, 32)
        Me.Button2.TabIndex = 33
        Me.Button2.Text = "終　了"
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(128, Byte), CType(128, Byte))
        Me.Button3.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.Location = New System.Drawing.Point(216, 288)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(120, 32)
        Me.Button3.TabIndex = 34
        Me.Button3.Text = "ﾃﾞﾌｫﾙﾄ設定"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(216, 248)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(232, 16)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "色相：xxBG RBGR　16進数で入力"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label8.Location = New System.Drawing.Point(168, 48)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(128, 16)
        Me.Label8.TabIndex = 36
        Me.Label8.Text = "（最大分解能:1023）"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label9.Location = New System.Drawing.Point(216, 264)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(232, 16)
        Me.Label9.TabIndex = 37
        Me.Label9.Text = "00で終了し最初から繰り返す"
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label10.Location = New System.Drawing.Point(328, 48)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(128, 16)
        Me.Label10.TabIndex = 38
        Me.Label10.Text = "（最速：１　最遅：256）"
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label11.Location = New System.Drawing.Point(400, 104)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 16)
        Me.Label11.TabIndex = 39
        Me.Label11.Text = "（1～1023）"
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label13.Location = New System.Drawing.Point(304, 136)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(136, 16)
        Me.Label13.TabIndex = 40
        Me.Label13.Text = "(PWM分解能～65000)"
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label14.Location = New System.Drawing.Point(8, 16)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(112, 24)
        Me.Label14.TabIndex = 41
        Me.Label14.Text = "COMポート指定"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label15.Location = New System.Drawing.Point(16, 128)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(96, 24)
        Me.Label15.TabIndex = 42
        Me.Label15.Text = "接続結果"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label16.Location = New System.Drawing.Point(144, 72)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(88, 24)
        Me.Label16.TabIndex = 43
        Me.Label16.Text = "最高照度"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Max
        '
        Me.Max.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Max.Location = New System.Drawing.Point(240, 72)
        Me.Max.Name = "Max"
        Me.Max.Size = New System.Drawing.Size(56, 26)
        Me.Max.TabIndex = 44
        Me.Max.Text = "1023"
        Me.Max.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label17.Location = New System.Drawing.Point(240, 104)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(72, 16)
        Me.Label17.TabIndex = 39
        Me.Label17.Text = "（1～1023）"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(224, Byte), CType(192, Byte))
        Me.ClientSize = New System.Drawing.Size(480, 334)
        Me.Controls.Add(Me.Max)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Setup)
        Me.Controls.Add(Me.Pattern15)
        Me.Controls.Add(Me.Pattern14)
        Me.Controls.Add(Me.Pattern13)
        Me.Controls.Add(Me.Pattern12)
        Me.Controls.Add(Me.Pattern11)
        Me.Controls.Add(Me.Pattern10)
        Me.Controls.Add(Me.Pattern9)
        Me.Controls.Add(Me.Pattern8)
        Me.Controls.Add(Me.Pattern7)
        Me.Controls.Add(Me.Pattern6)
        Me.Controls.Add(Me.Pattern5)
        Me.Controls.Add(Me.Pattern4)
        Me.Controls.Add(Me.Pattern3)
        Me.Controls.Add(Me.Pattern2)
        Me.Controls.Add(Me.Pattern1)
        Me.Controls.Add(Me.Pattern0)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.ChangeInterval)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Min)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Interval)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Period)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label17)
        Me.Name = "Form1"
        Me.Text = "イルミネーション"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub TimeOutSet()
        'タイムアウト構造体データ設定
        timeOut.ReadIntervalTimeout = 10
        timeOut.ReadTotalTimeoutConstant = 30
        timeOut.ReadTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutConstant = 30
        bRet = SetCommTimeouts(hComm, timeOut)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '初期化
        CloseHandle(0)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'USB接続制御
        Dim N As Integer
        bRet = CloseHandle(hComm)
        CommName = TextBox1.Text                        'COMポート番号取得
        hComm = CreateFile(CommName, &HC0000000, 0, IntPtr.Zero, &H3, &H80, IntPtr.Zero)
        If hComm <> -1 Then
            'DCB構造体データ設定
            stDCB.BaudRate = 19200
            stDCB.fBitFields = &H1
            stDCB.ByteSize = 8
            stDCB.Parity = 0
            stDCB.StopBits = 0
            bRet = SetCommState(hComm, stDCB)
            TimeOutSet()
            'デバイス接続確認 コマンド「0」の応答「OK」をチェック
            If bRet = True Then
                Application.DoEvents()
                wDATA = Chr(&H30)                       'コマンド0　OKが返信
                dLen = 1
                'コマンド送信後　折り返し受信
                bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
                bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
                If bRet = True And rLen = 3 Then
                    ' 返信データ　"OK"　を確認
                    If rDATA(0) = &H4F And rDATA(1) = &H4B Then
                        TextBox2.Text = "Connect"      '正常接続完了
                    Else
                        TextBox2.Text = "False"         '接続異常
                    End If
                Else
                    TextBox2.Text = "NoAns"             '応答なし
                End If
            Else
                TextBox2.Text = "Fault"                 '接続異常
            End If
        Else
            TextBox2.Text = "NoExist"
        End If
    End Sub

    Private Sub Setup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Setup.Click
        '新規設定データ送信制御
        wDATA = Chr(&H31)                                           'コマンド
        '全体周期件分解能　1023以下であること
        If Val(Period.Text) > 1023 Then
            Period.Text = "1023"
        End If
        wDATA = wDATA & Chr(CByte(Val(Period.Text) \ 256))            'Period
        wDATA = wDATA & Chr(CByte(Val(Period.Text) Mod 256))
        '更新周期
        If Val(Interval.Text) > 255 Then
            Interval.Text = "255"
        End If
        wDATA = wDATA & Chr(CByte(Val(Interval.Text) \ 256))          'Interval
        wDATA = wDATA & Chr(CByte(Val(Interval.Text) Mod 256))
        '照度最大値
        If Val(Max.Text) > Val(Period.Text) Then
            Max.Text = Period.Text
        End If
        wDATA = wDATA & Chr(CByte(Val(Max.Text) \ 256))               'Max
        wDATA = wDATA & Chr(CByte(Val(Max.Text) Mod 256))
        wDATA = wDATA & Chr(0)
        wDATA = wDATA & Chr(0)
        '照度最小値
        If Val(Min.Text) < 1 Then
            Min.Text = "1"
        End If
        wDATA = wDATA & Chr(CByte(Val(Min.Text) \ 256))               'Min
        wDATA = wDATA & Chr(CByte(Val(Min.Text) Mod 256))
        '色相周期
        wDATA = wDATA & Chr(CByte(Val(ChangeInterval.Text) \ 256))   'ChangeInterval
        wDATA = wDATA & Chr(CByte(Val(ChangeInterval.Text) Mod 256))
        wDATA = wDATA & Chr(0)                                       'dumy
        wDATA = wDATA & Chr(0)
        wDATA = wDATA & Chr(0)
        wDATA = wDATA & Chr(0)
        '色相パターン　１６組
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern0.Text)))        'Pattern0 to Pattern15
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern1.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern2.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern3.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern4.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern5.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern6.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern7.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern8.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern9.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern10.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern11.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern12.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern13.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern14.Text)))
        wDATA = wDATA & Chr(CByte(Val("&H" & Pattern15.Text))) & Chr(&H0)
        '新規設定データ送信
        dLen = 33
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '終了処理
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'デフォルト設定制御
        Application.DoEvents()
        wDATA = Chr(&H39)
        dLen = 1
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub

End Class

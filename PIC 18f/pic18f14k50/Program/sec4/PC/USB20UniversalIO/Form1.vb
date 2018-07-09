Public Class Form1
    Inherits System.Windows.Forms.Form
    Dim hComm As Integer                'COMポート用ハンドル
    Dim stDCB As DCB                    'DCBインスタンス
    Dim timeOut As COMMTIMEOUTS         'タイムアウト用インスタンス
    Dim wDATA As String                 'COM送信バッファ
    Dim rDATA(100) As Byte              'COM受信バッファ
    Dim dLen, wLen, rLen As Int32       'COM用パラメータ
    Dim bRet As Boolean                 '関数戻り値
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
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox6 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBox15 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox16 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox17 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox18 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox19 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox20 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox21 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox22 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Button14 As System.Windows.Forms.Button
    Friend WithEvents LATD0 As System.Windows.Forms.Button
    Friend WithEvents LATD1 As System.Windows.Forms.Button
    Friend WithEvents LATD2 As System.Windows.Forms.Button
    Friend WithEvents LATD3 As System.Windows.Forms.Button
    Friend WithEvents RD0 As System.Windows.Forms.TextBox
    Friend WithEvents RD1 As System.Windows.Forms.TextBox
    Friend WithEvents RD2 As System.Windows.Forms.TextBox
    Friend WithEvents RD3 As System.Windows.Forms.TextBox
    Friend WithEvents RD4 As System.Windows.Forms.TextBox
    Friend WithEvents RD5 As System.Windows.Forms.TextBox
    Friend WithEvents RD6 As System.Windows.Forms.TextBox
    Friend WithEvents RD7 As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button7 = New System.Windows.Forms.Button
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Button9 = New System.Windows.Forms.Button
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.TextBox6 = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Button13 = New System.Windows.Forms.Button
        Me.LATD0 = New System.Windows.Forms.Button
        Me.LATD1 = New System.Windows.Forms.Button
        Me.LATD2 = New System.Windows.Forms.Button
        Me.LATD3 = New System.Windows.Forms.Button
        Me.RD0 = New System.Windows.Forms.TextBox
        Me.RD1 = New System.Windows.Forms.TextBox
        Me.RD2 = New System.Windows.Forms.TextBox
        Me.RD3 = New System.Windows.Forms.TextBox
        Me.RD4 = New System.Windows.Forms.TextBox
        Me.RD5 = New System.Windows.Forms.TextBox
        Me.RD6 = New System.Windows.Forms.TextBox
        Me.RD7 = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Button14 = New System.Windows.Forms.Button
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextBox22 = New System.Windows.Forms.TextBox
        Me.TextBox21 = New System.Windows.Forms.TextBox
        Me.TextBox20 = New System.Windows.Forms.TextBox
        Me.TextBox19 = New System.Windows.Forms.TextBox
        Me.TextBox18 = New System.Windows.Forms.TextBox
        Me.TextBox17 = New System.Windows.Forms.TextBox
        Me.TextBox16 = New System.Windows.Forms.TextBox
        Me.TextBox15 = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Lime
        Me.Button1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(40, 96)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "LED2"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Lime
        Me.Button2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(160, 96)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 32)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "LED3"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label2.Location = New System.Drawing.Point(24, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "LED制御"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(128, 336)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(168, 23)
        Me.TextBox2.TabIndex = 4
        Me.TextBox2.Text = "Message"
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(128, 272)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(168, 23)
        Me.TextBox3.TabIndex = 5
        Me.TextBox3.Text = "Mesure Data"
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button3.Location = New System.Drawing.Point(32, 336)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(88, 32)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "MSG受信"
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button4.Location = New System.Drawing.Point(32, 272)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(88, 32)
        Me.Button4.TabIndex = 7
        Me.Button4.Text = "連続計測"
        '
        'Button5
        '
        Me.Button5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button5.Location = New System.Drawing.Point(32, 160)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(96, 32)
        Me.Button5.TabIndex = 9
        Me.Button5.Text = "表示出力"
        '
        'Button6
        '
        Me.Button6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button6.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button6.Location = New System.Drawing.Point(392, 352)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(88, 32)
        Me.Button6.TabIndex = 10
        Me.Button6.Text = "終了"
        '
        'Timer1
        '
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(40, 32)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(64, 23)
        Me.TextBox1.TabIndex = 11
        Me.TextBox1.Text = "COM7"
        '
        'Button7
        '
        Me.Button7.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Button7.Location = New System.Drawing.Point(112, 24)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(88, 40)
        Me.Button7.TabIndex = 12
        Me.Button7.Text = "USB接続"
        '
        'TextBox4
        '
        Me.TextBox4.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(224, 32)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(64, 23)
        Me.TextBox4.TabIndex = 13
        Me.TextBox4.Text = "False"
        '
        'Label1
        '
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label1.Location = New System.Drawing.Point(40, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 16)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "COMポート"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label3
        '
        Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label3.Location = New System.Drawing.Point(216, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "接続結果"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label4.Location = New System.Drawing.Point(24, 144)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(112, 16)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "液晶表示器の制御"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Button9
        '
        Me.Button9.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button9.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button9.Location = New System.Drawing.Point(152, 160)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(88, 32)
        Me.Button9.TabIndex = 18
        Me.Button9.Text = "表示消去"
        '
        'TextBox5
        '
        Me.TextBox5.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(56, 216)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(40, 23)
        Me.TextBox5.TabIndex = 19
        Me.TextBox5.Text = "C0"
        '
        'TextBox6
        '
        Me.TextBox6.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox6.Location = New System.Drawing.Point(120, 216)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(176, 23)
        Me.TextBox6.TabIndex = 20
        Me.TextBox6.Text = "Out Message 01234567"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label5.Location = New System.Drawing.Point(24, 256)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 16)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "データ受信"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label6.Location = New System.Drawing.Point(24, 320)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 16)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "メッセージ受信"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label7.Location = New System.Drawing.Point(48, 200)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(56, 16)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Command"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Label8.Location = New System.Drawing.Point(120, 200)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 16)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "メッセージ"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button13)
        Me.GroupBox1.Controls.Add(Me.LATD0)
        Me.GroupBox1.Controls.Add(Me.LATD1)
        Me.GroupBox1.Controls.Add(Me.LATD2)
        Me.GroupBox1.Controls.Add(Me.LATD3)
        Me.GroupBox1.Controls.Add(Me.RD0)
        Me.GroupBox1.Controls.Add(Me.RD1)
        Me.GroupBox1.Controls.Add(Me.RD2)
        Me.GroupBox1.Controls.Add(Me.RD3)
        Me.GroupBox1.Controls.Add(Me.RD4)
        Me.GroupBox1.Controls.Add(Me.RD5)
        Me.GroupBox1.Controls.Add(Me.RD6)
        Me.GroupBox1.Controls.Add(Me.RD7)
        Me.GroupBox1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.GroupBox1.Location = New System.Drawing.Point(328, 24)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(224, 128)
        Me.GroupBox1.TabIndex = 25
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "ディジタルI/O"
        '
        'Button13
        '
        Me.Button13.Location = New System.Drawing.Point(136, 48)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(72, 32)
        Me.Button13.TabIndex = 12
        Me.Button13.Text = "制御"
        '
        'LATD0
        '
        Me.LATD0.Location = New System.Drawing.Point(88, 88)
        Me.LATD0.Name = "LATD0"
        Me.LATD0.Size = New System.Drawing.Size(24, 24)
        Me.LATD0.TabIndex = 11
        Me.LATD0.Text = "０"
        '
        'LATD1
        '
        Me.LATD1.Location = New System.Drawing.Point(64, 88)
        Me.LATD1.Name = "LATD1"
        Me.LATD1.Size = New System.Drawing.Size(24, 24)
        Me.LATD1.TabIndex = 10
        Me.LATD1.Text = "１"
        '
        'LATD2
        '
        Me.LATD2.Location = New System.Drawing.Point(40, 88)
        Me.LATD2.Name = "LATD2"
        Me.LATD2.Size = New System.Drawing.Size(24, 24)
        Me.LATD2.TabIndex = 9
        Me.LATD2.Text = "２"
        '
        'LATD3
        '
        Me.LATD3.Location = New System.Drawing.Point(16, 88)
        Me.LATD3.Name = "LATD3"
        Me.LATD3.Size = New System.Drawing.Size(24, 24)
        Me.LATD3.TabIndex = 8
        Me.LATD3.Text = "３"
        '
        'RD0
        '
        Me.RD0.BackColor = System.Drawing.Color.Lime
        Me.RD0.Location = New System.Drawing.Point(88, 56)
        Me.RD0.Name = "RD0"
        Me.RD0.Size = New System.Drawing.Size(24, 23)
        Me.RD0.TabIndex = 7
        Me.RD0.Text = "Of"
        Me.RD0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RD1
        '
        Me.RD1.BackColor = System.Drawing.Color.Lime
        Me.RD1.Location = New System.Drawing.Point(64, 56)
        Me.RD1.Name = "RD1"
        Me.RD1.Size = New System.Drawing.Size(24, 23)
        Me.RD1.TabIndex = 6
        Me.RD1.Text = "Of"
        Me.RD1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RD2
        '
        Me.RD2.BackColor = System.Drawing.Color.Lime
        Me.RD2.Location = New System.Drawing.Point(40, 56)
        Me.RD2.Name = "RD2"
        Me.RD2.Size = New System.Drawing.Size(24, 23)
        Me.RD2.TabIndex = 5
        Me.RD2.Text = "Of"
        Me.RD2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RD3
        '
        Me.RD3.BackColor = System.Drawing.Color.Lime
        Me.RD3.Location = New System.Drawing.Point(16, 56)
        Me.RD3.Name = "RD3"
        Me.RD3.Size = New System.Drawing.Size(24, 23)
        Me.RD3.TabIndex = 4
        Me.RD3.Text = "Of"
        Me.RD3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RD4
        '
        Me.RD4.BackColor = System.Drawing.Color.Lime
        Me.RD4.Location = New System.Drawing.Point(88, 24)
        Me.RD4.Name = "RD4"
        Me.RD4.Size = New System.Drawing.Size(24, 23)
        Me.RD4.TabIndex = 3
        Me.RD4.Text = "Of"
        Me.RD4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RD5
        '
        Me.RD5.BackColor = System.Drawing.Color.Lime
        Me.RD5.Location = New System.Drawing.Point(64, 24)
        Me.RD5.Name = "RD5"
        Me.RD5.Size = New System.Drawing.Size(24, 23)
        Me.RD5.TabIndex = 2
        Me.RD5.Text = "Of"
        Me.RD5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RD6
        '
        Me.RD6.BackColor = System.Drawing.Color.Lime
        Me.RD6.Location = New System.Drawing.Point(40, 24)
        Me.RD6.Name = "RD6"
        Me.RD6.Size = New System.Drawing.Size(24, 23)
        Me.RD6.TabIndex = 1
        Me.RD6.Text = "Of"
        Me.RD6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RD7
        '
        Me.RD7.BackColor = System.Drawing.Color.Lime
        Me.RD7.Location = New System.Drawing.Point(16, 24)
        Me.RD7.Name = "RD7"
        Me.RD7.Size = New System.Drawing.Size(24, 23)
        Me.RD7.TabIndex = 0
        Me.RD7.Text = "Of"
        Me.RD7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button14)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.TextBox22)
        Me.GroupBox2.Controls.Add(Me.TextBox21)
        Me.GroupBox2.Controls.Add(Me.TextBox20)
        Me.GroupBox2.Controls.Add(Me.TextBox19)
        Me.GroupBox2.Controls.Add(Me.TextBox18)
        Me.GroupBox2.Controls.Add(Me.TextBox17)
        Me.GroupBox2.Controls.Add(Me.TextBox16)
        Me.GroupBox2.Controls.Add(Me.TextBox15)
        Me.GroupBox2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.GroupBox2.Location = New System.Drawing.Point(328, 160)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(224, 168)
        Me.GroupBox2.TabIndex = 26
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "アナログ計測"
        '
        'Button14
        '
        Me.Button14.Location = New System.Drawing.Point(56, 128)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(104, 32)
        Me.Button14.TabIndex = 16
        Me.Button14.Text = "データ入力"
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(168, 96)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(32, 24)
        Me.Label16.TabIndex = 15
        Me.Label16.Text = "AN7"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(168, 72)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(32, 24)
        Me.Label15.TabIndex = 14
        Me.Label15.Text = "AN6"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(168, 48)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(32, 24)
        Me.Label14.TabIndex = 13
        Me.Label14.Text = "AN5"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(168, 24)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(32, 24)
        Me.Label13.TabIndex = 12
        Me.Label13.Text = "AN4"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(64, 96)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(40, 24)
        Me.Label12.TabIndex = 11
        Me.Label12.Text = "AN3"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(64, 72)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(32, 24)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "AN2"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(64, 48)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(32, 24)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "AN1"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label9.Location = New System.Drawing.Point(64, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(32, 24)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "AN0"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox22
        '
        Me.TextBox22.Location = New System.Drawing.Point(120, 96)
        Me.TextBox22.Name = "TextBox22"
        Me.TextBox22.Size = New System.Drawing.Size(40, 23)
        Me.TextBox22.TabIndex = 7
        Me.TextBox22.Text = ""
        Me.TextBox22.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox21
        '
        Me.TextBox21.Location = New System.Drawing.Point(120, 72)
        Me.TextBox21.Name = "TextBox21"
        Me.TextBox21.Size = New System.Drawing.Size(40, 23)
        Me.TextBox21.TabIndex = 6
        Me.TextBox21.Text = ""
        Me.TextBox21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox20
        '
        Me.TextBox20.Location = New System.Drawing.Point(120, 48)
        Me.TextBox20.Name = "TextBox20"
        Me.TextBox20.Size = New System.Drawing.Size(40, 23)
        Me.TextBox20.TabIndex = 5
        Me.TextBox20.Text = ""
        Me.TextBox20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox19
        '
        Me.TextBox19.Location = New System.Drawing.Point(120, 24)
        Me.TextBox19.Name = "TextBox19"
        Me.TextBox19.Size = New System.Drawing.Size(40, 23)
        Me.TextBox19.TabIndex = 4
        Me.TextBox19.Text = ""
        Me.TextBox19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox18
        '
        Me.TextBox18.Location = New System.Drawing.Point(16, 96)
        Me.TextBox18.Name = "TextBox18"
        Me.TextBox18.Size = New System.Drawing.Size(40, 23)
        Me.TextBox18.TabIndex = 3
        Me.TextBox18.Text = ""
        Me.TextBox18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox17
        '
        Me.TextBox17.Location = New System.Drawing.Point(16, 72)
        Me.TextBox17.Name = "TextBox17"
        Me.TextBox17.Size = New System.Drawing.Size(40, 23)
        Me.TextBox17.TabIndex = 2
        Me.TextBox17.Text = ""
        Me.TextBox17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox16
        '
        Me.TextBox16.Location = New System.Drawing.Point(16, 48)
        Me.TextBox16.Name = "TextBox16"
        Me.TextBox16.Size = New System.Drawing.Size(40, 23)
        Me.TextBox16.TabIndex = 1
        Me.TextBox16.Text = ""
        Me.TextBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox15
        '
        Me.TextBox15.Location = New System.Drawing.Point(16, 24)
        Me.TextBox15.Name = "TextBox15"
        Me.TextBox15.Size = New System.Drawing.Size(40, 23)
        Me.TextBox15.TabIndex = 0
        Me.TextBox15.Text = ""
        Me.TextBox15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.Color.Teal
        Me.ClientSize = New System.Drawing.Size(576, 398)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox6)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.Text = "Universal I/O"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TimeOutSet()
        'タイムアウト用構造体データ設定
        timeOut.ReadIntervalTimeout = 10
        timeOut.ReadTotalTimeoutConstant = 30
        timeOut.ReadTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutConstant = 30
        bRet = SetCommTimeouts(hComm, timeOut)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '初期化
        TextBox4.Text = ""
        Timer1.Stop()
        CloseHandle(0)
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        'USB接続制御
        Dim N As Integer
        CommName = TextBox1.Text        'COMポート番号取得
        bRet = CloseHandle(hComm)
        'COMポートオープン
        hComm = CreateFile(CommName, &HC0000000, 0, IntPtr.Zero, &H3, &H80, IntPtr.Zero)
        If hComm <> -1 Then
            '正常オープンできたらDCB設定
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
                wDATA = Chr(&H30)           'コマンド0　OKが返信
                dLen = 1
                'コマンド送信後　折り返し受信
                bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
                bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
                If bRet = True And rLen = 3 Then
                    ' 返信データ　"OK"　を確認
                    If rDATA(0) = &H4F And rDATA(1) = &H4B Then
                        TextBox4.Text = "Connect"      '正常接続完了
                    Else
                        TextBox4.Text = "False"     '接続異常
                    End If
                Else
                    TextBox4.Text = "NoAns"             '応答なし
                End If
            Else
                TextBox4.Text = "Fault"             '接続異常
            End If
        Else
            TextBox4.Text = "NoExist"
        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'LEDオンオフ制御
        Application.DoEvents()
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H31)           'コマンド
        dLen = Len(wDATA)
        'コマンド送信後折り返しの状態受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            '受信した状態でボタンの色を変える
            If Chr(rDATA(0)) = Chr(&H30) Then
                Button1.BackColor() = System.Drawing.Color.Red
            Else
                Button1.BackColor = System.Drawing.Color.Lime
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'LEDのオンオフ制御
        Application.DoEvents()
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H32)           'コマンド
        dLen = Len(wDATA)
        'コマンド送信後折り返しの状態受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            '受信した状態でボタンの色を変える
            If Chr(rDATA(0)) = Chr(&H30) Then
                Button2.BackColor() = System.Drawing.Color.Red
            Else
                Button2.BackColor = System.Drawing.Color.Lime
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'メッセージ受信制御
        Dim N As Integer
        Application.DoEvents()
        TextBox2.Clear()
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H38)           'コマンド
        dLen = Len(wDATA)
        'コマンド送信後　折り返しのメッセージを受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            '受信メッセージを表示
            For N = 0 To rLen - 1 Step 1
                TextBox2.Text = TextBox2.Text & Chr(rDATA(N))
            Next
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '連続計測制御　POTの入力
        Dim N As Integer
        Application.DoEvents()
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H35)               'コマンド
        dLen = Len(wDATA)
        'コマンドの送信後、折り返しのPOTデータ受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            'POTデータ表示
            For N = 0 To rLen - 1 Step 1
                TextBox3.Text = TextBox3.Text & Chr(rDATA(N))
            Next
        End If
        'この後一定時間間隔で繰り返し
        Timer1.Interval() = 200
        Timer1.Start()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'メッセージの送信　テキストボックス内のメッセージを送信
        wDATA = Chr(&H33) & TextBox5.Text & TextBox6.Text & Chr(0)
        dLen = Len(wDATA)
        '送信実行
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        '終了処理
        Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'タイマによる一定間隔でのPOTデータの表示
        Dim N As Integer
        Application.DoEvents()
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H35)           'コマンド送信
        dLen = Len(wDATA)
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            'POTデータの再表示
            For N = 0 To rLen - 1 Step 1
                TextBox3.Text = TextBox3.Text & Chr(rDATA(N))
            Next
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        '液晶表示器の表示消去
        wDATA = Chr(&H34)
        dLen = Len(wDATA)
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub


    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        'ディジタル制御　8ビットパラレル
        Dim State As Byte
        '下位４ビットの制御内容設定
        State = 0
        If RD0.Text = "On" Then
            State = State Or &H1
        Else
            State = State And &HFE
        End If
        If RD1.Text = "On" Then
            State = State Or &H2
        Else
            State = State And &HFD
        End If
        If RD2.Text = "On" Then
            State = State Or &H4
        Else
            State = State And &HFB
        End If
        If RD3.Text = "On" Then
            State = State Or &H8
        Else
            State = State And &HF7
        End If
        '送信後　折り返しの状態データ受信
        wDATA = Chr(&H36) & Chr(State)
        dLen = Len(wDATA)
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            '8ビットの状態を各窓に色と文字で表示
            If (rDATA(0) And &H1) = 0 Then
                RD0.BackColor = System.Drawing.Color.Lime
                RD0.Text = "Of"
            Else
                RD0.BackColor = System.Drawing.Color.Red
                RD0.Text = "On"
            End If
            If (rDATA(0) And &H2) = 0 Then
                RD1.BackColor = System.Drawing.Color.Lime
                RD1.Text = "Of"
            Else
                RD1.BackColor = System.Drawing.Color.Red
                RD1.Text = "On"
            End If
            If (rDATA(0) And &H4) = 0 Then
                RD2.BackColor = System.Drawing.Color.Lime
                RD2.Text = "Of"
            Else
                RD2.BackColor = System.Drawing.Color.Red
                RD2.Text = "On"
            End If
            If (rDATA(0) And &H8) = 0 Then
                RD3.BackColor = System.Drawing.Color.Lime
                RD3.Text = "Of"
            Else
                RD3.BackColor = System.Drawing.Color.Red
                RD3.Text = "On"
            End If
            If (rDATA(0) And &H10) = 0 Then
                RD4.BackColor = System.Drawing.Color.Lime
                RD4.Text = "Of"
            Else
                RD4.BackColor = System.Drawing.Color.Red
                RD4.Text = "On"
            End If
            If (rDATA(0) And &H20) = 0 Then
                RD5.BackColor = System.Drawing.Color.Lime
                RD5.Text = "Of"
            Else
                RD5.BackColor = System.Drawing.Color.Red
                RD5.Text = "On"
            End If
            If (rDATA(0) And &H40) = 0 Then
                RD6.BackColor = System.Drawing.Color.Lime
                RD6.Text = "Of"
            Else
                RD6.BackColor = System.Drawing.Color.Red
                RD6.Text = "On"
            End If
            If (rDATA(0) And &H80) = 0 Then
                RD7.BackColor = System.Drawing.Color.Lime
                RD7.Text = "Of"
            Else
                RD7.BackColor = System.Drawing.Color.Red
                RD7.Text = "On"
            End If


        End If
    End Sub

    Private Sub LATD0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LATD0.Click
        '下位４ビットの制御指示　色と文字を変更
        If RD0.Text = "Of" Then
            RD0.BackColor = System.Drawing.Color.Red
            RD0.Text = "On"
        Else
            RD0.BackColor = System.Drawing.Color.Lime
            RD0.Text = "Of"
        End If
    End Sub

    Private Sub LATD1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LATD1.Click
        '下位４ビットの制御指示　色と文字を変更
        If RD1.Text = "Of" Then
            RD1.BackColor = System.Drawing.Color.Red
            RD1.Text = "On"
        Else
            RD1.BackColor = System.Drawing.Color.Lime
            RD1.Text = "Of"
        End If
    End Sub

    Private Sub LATD2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LATD2.Click
        '下位４ビットの制御指示　色と文字を変更
        If RD2.Text = "Of" Then
            RD2.BackColor = System.Drawing.Color.Red
            RD2.Text = "On"
        Else
            RD2.BackColor = System.Drawing.Color.Lime
            RD2.Text = "Of"
        End If
    End Sub

    Private Sub LATD3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LATD3.Click
        '下位４ビットの制御指示　色と文字を変更
        If RD3.Text = "Of" Then
            RD3.BackColor = System.Drawing.Color.Red
            RD3.Text = "On"
        Else
            RD3.BackColor = System.Drawing.Color.Lime
            RD3.Text = "Of"
        End If
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        '８チャネルの計測制御
        Dim N As Integer

        wDATA = Chr(&H37)           'コマンド
        dLen = Len(wDATA)
        'コマンド送信後　折り返しの８チャネルのデータ受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 32, rLen, IntPtr.Zero)
        If bRet = True Then
            '受信した計測データを文字列にして表示
            TextBox15.Text = ""
            For N = 0 To 3 Step 1
                TextBox15.Text = TextBox15.Text & Chr(rDATA(N))
            Next
            TextBox16.Text = ""
            For N = 0 To 3 Step 1
                TextBox16.Text = TextBox16.Text & Chr(rDATA(N + 4))
            Next
            TextBox17.Text = ""
            For N = 0 To 3 Step 1
                TextBox17.Text = TextBox17.Text & Chr(rDATA(N + 8))
            Next
            TextBox18.Text = ""
            For N = 0 To 3 Step 1
                TextBox18.Text = TextBox18.Text & Chr(rDATA(N + 12))
            Next
            TextBox19.Text = ""
            For N = 0 To 3 Step 1
                TextBox19.Text = TextBox19.Text & Chr(rDATA(N + 16))
            Next
            TextBox20.Text = ""
            For N = 0 To 3 Step 1
                TextBox20.Text = TextBox20.Text & Chr(rDATA(N + 20))
            Next
            TextBox21.Text = ""
            For N = 0 To 3 Step 1
                TextBox21.Text = TextBox21.Text & Chr(rDATA(N + 24))
            Next
            TextBox22.Text = ""
            For N = 0 To 3 Step 1
                TextBox22.Text = TextBox22.Text & Chr(rDATA(N + 28))
            Next

        End If

    End Sub
End Class

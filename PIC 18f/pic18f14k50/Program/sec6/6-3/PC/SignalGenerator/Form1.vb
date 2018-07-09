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
    Dim Frequency As Integer            '周波数データ


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
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Button11 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button10 = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button9 = New System.Windows.Forms.Button
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Button11 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("ＭＳ ゴシック", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(48, 128)
        Me.TextBox1.MaxLength = 7
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.TextBox1.Size = New System.Drawing.Size(152, 55)
        Me.TextBox1.TabIndex = 6
        Me.TextBox1.Text = "123456"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(128, Byte), CType(64, Byte), CType(0, Byte))
        Me.Button1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(264, 136)
        Me.Button1.Name = "Button1"
        Me.Button1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button1.Size = New System.Drawing.Size(112, 32)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "正弦波出力"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(192, Byte), CType(64, Byte), CType(0, Byte))
        Me.Button2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(176, 104)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(24, 24)
        Me.Button2.TabIndex = 8
        Me.Button2.Text = "▲"
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.Blue
        Me.Button3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.Location = New System.Drawing.Point(176, 184)
        Me.Button3.Name = "Button3"
        Me.Button3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button3.Size = New System.Drawing.Size(24, 24)
        Me.Button3.TabIndex = 9
        Me.Button3.Text = "▼"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(192, Byte), CType(64, Byte), CType(0, Byte))
        Me.Button4.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.Location = New System.Drawing.Point(128, 104)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(24, 24)
        Me.Button4.TabIndex = 10
        Me.Button4.Text = "▲"
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.Blue
        Me.Button5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.Location = New System.Drawing.Point(128, 184)
        Me.Button5.Name = "Button5"
        Me.Button5.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button5.Size = New System.Drawing.Size(24, 24)
        Me.Button5.TabIndex = 11
        Me.Button5.Text = "▼"
        Me.Button5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.FromArgb(CType(192, Byte), CType(64, Byte), CType(0, Byte))
        Me.Button6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button6.Location = New System.Drawing.Point(80, 104)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(24, 24)
        Me.Button6.TabIndex = 12
        Me.Button6.Text = "▲"
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.Color.Blue
        Me.Button7.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button7.Location = New System.Drawing.Point(80, 184)
        Me.Button7.Name = "Button7"
        Me.Button7.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Button7.Size = New System.Drawing.Size(24, 24)
        Me.Button7.TabIndex = 13
        Me.Button7.Text = "▼"
        Me.Button7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(200, 144)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label1.Size = New System.Drawing.Size(40, 32)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Hz"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Button10
        '
        Me.Button10.BackColor = System.Drawing.Color.Olive
        Me.Button10.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button10.Location = New System.Drawing.Point(264, 176)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(112, 32)
        Me.Button10.TabIndex = 17
        Me.Button10.Text = "三角波出力"
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.Color.Navy
        Me.Button8.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button8.Location = New System.Drawing.Point(264, 96)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(112, 32)
        Me.Button8.TabIndex = 18
        Me.Button8.Text = "現在値入力"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(16, 24)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(48, 23)
        Me.TextBox2.TabIndex = 19
        Me.TextBox2.Text = "COM6"
        '
        'Button9
        '
        Me.Button9.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button9.Location = New System.Drawing.Point(80, 24)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(80, 32)
        Me.Button9.TabIndex = 20
        Me.Button9.Text = "USB接続"
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(176, 24)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(72, 23)
        Me.TextBox3.TabIndex = 21
        Me.TextBox3.Text = "Fault"
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button11
        '
        Me.Button11.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button11.Location = New System.Drawing.Point(296, 24)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(80, 32)
        Me.Button11.TabIndex = 22
        Me.Button11.Text = "終了"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.Color.Teal
        Me.ClientSize = New System.Drawing.Size(392, 222)
        Me.Controls.Add(Me.Button11)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.ForeColor = System.Drawing.Color.White
        Me.Name = "Form1"
        Me.Text = "Signal Generator"
        Me.ResumeLayout(False)

    End Sub

#End Region

    'COM用タイムアウト関数
    Private Sub TimeOutSet()
        timeOut.ReadIntervalTimeout = 20
        timeOut.ReadTotalTimeoutConstant = 50
        timeOut.ReadTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutConstant = 10
        bRet = SetCommTimeouts(hComm, timeOut)
    End Sub

    '初期化
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        'USB接続確認制御
        Dim N As Integer
        CommName = TextBox2.Text            'COMポート番号取得
        bRet = CloseHandle(hComm)
        'COMポートオープン
        hComm = CreateFile(CommName, &HC0000000, 0, IntPtr.Zero, &H3, &H80, IntPtr.Zero)
        If hComm <> -1 Then
            'DCB構造体データ設定
            stDCB.BaudRate = 115200
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
                        TextBox3.Text = "Connect"      '正常接続完了
                        '周波数現在値入力表示制御
                        wDATA = Chr(&H31)           'コマンド
                        dLen = Len(wDATA)
                        'コマンド送信後、周波数データ受信
                        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
                        bRet = ReadFile(hComm, rDATA, 6, rLen, IntPtr.Zero)
                        If bRet = True Then
                            '周波数データ表示
                            TextBox1.Text = ""
                            For N = 0 To rLen Step 1
                                TextBox1.Text = TextBox1.Text & Chr(rDATA(N))
                            Next
                        End If
                        Frequency = Val(TextBox1.Text & Chr(0))
                    Else
                        TextBox3.Text = "False"     '接続異常
                    End If
                Else
                    TextBox3.Text = "NoAns"             '応答なし
                End If
            Else
                TextBox3.Text = "Fault"             '接続異常
            End If
        Else
            TextBox3.Text = "NoExist"
        End If
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '1Hz桁周波数アップ
        Frequency = Frequency + 1
        TextBox1.Text = Format(Frequency, "000000")

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '1Hz桁周波数ダウン
        Frequency = Frequency - 1
        TextBox1.Text = Format(Frequency, "000000")
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '100Hz桁周波数アップ
        Frequency = Frequency + 100
        TextBox1.Text = Format(Frequency, "000000")
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        '100Hz桁周波数ダウン
        Frequency = Frequency - 100
        TextBox1.Text = Format(Frequency, "000000")
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        '10kHz桁周波数アップ
        Frequency = Frequency + 10000
        TextBox1.Text = Format(Frequency, "000000")
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        '10kHz周波数桁ダウン
        Frequency = Frequency - 10000
        TextBox1.Text = Format(Frequency, "000000")
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '正弦波データ設定制御
        wDATA = Chr(&H32) & Format(Frequency, "000000") & Chr(0)
        dLen = Len(wDATA)
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        '三角波データ設定制御
        wDATA = Chr(&H33) & Format(Frequency, "000000") & Chr(0)
        dLen = Len(wDATA)
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub


    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        '周波数現在値入力表示制御
        Dim N As Integer
        wDATA = Chr(&H31)           'コマンド
        dLen = Len(wDATA)
        'コマンド送信後、周波数データ受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 6, rLen, IntPtr.Zero)
        If bRet = True Then
            '周波数データ表示
            TextBox1.Text = ""
            For N = 0 To rLen Step 1
                TextBox1.Text = TextBox1.Text & Chr(rDATA(N))
            Next
        End If
        Frequency = Val(TextBox1.Text & Chr(0))
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        '終了処理
        Close()
    End Sub
End Class

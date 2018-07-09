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
    Dim Flag As Integer                 '受信データバッファ用フラグ



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
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label5 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button4 = New System.Windows.Forms.Button
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button5 = New System.Windows.Forms.Button
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Button8 = New System.Windows.Forms.Button
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.Label5 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(32, 24)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 23)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "COM9"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(96, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 32)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "USB接続"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(192, 24)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(64, 23)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = "False"
        '
        'Timer1
        '
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(296, 208)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 32)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "終了"
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.Location = New System.Drawing.Point(40, 128)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(80, 32)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "測定開始"
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("ＭＳ ゴシック", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(136, 144)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(104, 28)
        Me.TextBox3.TabIndex = 5
        Me.TextBox3.Text = "00000000"
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("ＭＳ ゴシック", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(248, 152)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 24)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Hz"
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.Location = New System.Drawing.Point(144, 88)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(80, 32)
        Me.Button4.TabIndex = 7
        Me.Button4.Text = "10MHz"
        '
        'TextBox4
        '
        Me.TextBox4.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(40, 88)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(32, 23)
        Me.TextBox4.TabIndex = 8
        Me.TextBox4.Text = "1"
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(32, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "測定間隔"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(144, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 16)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "上限周波数切替"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Button5
        '
        Me.Button5.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.Location = New System.Drawing.Point(40, 160)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(80, 32)
        Me.Button5.TabIndex = 11
        Me.Button5.Text = "測定停止"
        '
        'Button6
        '
        Me.Button6.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button6.Location = New System.Drawing.Point(296, 128)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(80, 32)
        Me.Button6.TabIndex = 12
        Me.Button6.Text = "記録開始"
        '
        'Button7
        '
        Me.Button7.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button7.Location = New System.Drawing.Point(296, 160)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(80, 32)
        Me.Button7.TabIndex = 13
        Me.Button7.Text = "記録終了"
        '
        'Button8
        '
        Me.Button8.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button8.Location = New System.Drawing.Point(40, 208)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(80, 32)
        Me.Button8.TabIndex = 14
        Me.Button8.Text = "グラフ"
        '
        'TextBox5
        '
        Me.TextBox5.Font = New System.Drawing.Font("ＭＳ ゴシック", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(136, 208)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(96, 28)
        Me.TextBox5.TabIndex = 15
        Me.TextBox5.Text = "12800000"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.Location = New System.Drawing.Point(136, 192)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 16)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "基準値"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(80, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 16)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "秒"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.ClientSize = New System.Drawing.Size(400, 254)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "Frequency Counter"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TimeOutSet()
        'COMポート用タイムアウト関数
        timeOut.ReadIntervalTimeout = 10
        timeOut.ReadTotalTimeoutConstant = 30
        timeOut.ReadTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutConstant = 30
        bRet = SetCommTimeouts(hComm, timeOut)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'USB接続実行
        Dim N As Integer
        CommName = TextBox1.Text              'COMポート番号取得
        '        bRet = CloseHandle(hComm)
        'COMポートオープン
        hComm = CreateFile(CommName, &HC0000000, 0, IntPtr.Zero, &H3, &H80, IntPtr.Zero)
        If hComm <> -1 Then
            '正常接続完了した場合
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
                        TextBox2.Text = "Connect"      '正常接続完了
                    Else
                        TextBox2.Text = "False"     '接続異常
                    End If
                Else
                    TextBox2.Text = "NoAns"             '応答なし
                End If
            Else
                TextBox2.Text = "Fault"             '接続異常
            End If
        Else
            TextBox2.Text = "NoExist"
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '初期設定
        F1 = Me                 'フォームの名前設定
        TextBox2.Text = ""
        Button4.Text = "10MHz"
        Timer1.Stop()
        CloseHandle(0)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '終了処理
        Timer1.Stop()
        CloseHandle(0)
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '測定開始
        Dim N As Integer
        Application.DoEvents()
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H33)               '測定開始コマンド
        dLen = Len(wDATA)
        'コマンド送信、折り返しデータ周波数受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            '周波数データ表示
            For N = 0 To rLen - 3 Step 1
                TextBox3.Text = TextBox3.Text & Chr(rDATA(N))
            Next
        End If
        '繰り返し計測周期をタイマに設定と開始
        Timer1.Interval() = Val(TextBox4.Text) * 1000
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'タイマによる周期計測
        Dim N As Integer
        Application.DoEvents()
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H33)
        dLen = Len(wDATA)
        '計測コマンド送信と折り返し受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 30, rLen, IntPtr.Zero)
        If bRet = True Then
            '周波数表示
            For N = 0 To rLen - 3 Step 1
                TextBox3.Text = TextBox3.Text & Chr(rDATA(N))
            Next
            'データ保存指示があれば配列に格納
            If Flag = True Then
                mDATA(Index) = Val(TextBox3.Text)
                Index = Index + 1
                If Index > 10000 Then       '最大１万回まで保存
                    Flag = False
                    Timer1.Stop()
                    MessageBox.Show("バッファが一杯です。測定を停止します。")
                End If
            End If
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'バンド幅の切替
        If Button4.Text = "50MHz" Then
            wDATA = Chr(&H31)           'バンド幅50MHz　8Hz単位カウント
            dLen = Len(wDATA)
            bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
            Button4.Text = "10MHz"
        Else
            wDATA = Chr(&H32)           'バンド幅10MHz　1Hz単位
            dLen = Len(wDATA)
            bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
            Button4.Text = "50MHz"
        End If

    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        'グラフ表示指示　別フォームオープン
        F2.Show()
    End Sub


    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        '周期計測停止制御
        Timer1.Stop()

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        '周期計測データ保存制御
        Index = 0
        Flag = True
        Timer1.Interval() = Val(TextBox4.Text) * 1000
        Timer1.Start()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        '計測記録終了　ファイル保存
        Dim i As Integer
        Dim svFile1 As New SaveFileDialog
        Flag = False
        Timer1.Stop()
        'ファイルに保存
        svFile1.Filter = "data file(*.dat)|*dat|all file(*.*)|*.*"
        svFile1.FilterIndex = 1
        svFile1.RestoreDirectory = True

        bRet = MessageBox.Show("ファイルに保存します。")    'メッセージダイアログ
        Dim ret As DialogResult = svFile1.ShowDialog        'ファイルダイアログ
        If ret <> DialogResult.Cancel Then
            FileOpen(1, svFile1.FileName, OpenMode.Output)  '指定ファイルオープン
            'データ保存実行
            For i = 0 To Index
                Write(1, mDATA(i))
            Next
            FileClose(1)
        End If
    End Sub


End Class

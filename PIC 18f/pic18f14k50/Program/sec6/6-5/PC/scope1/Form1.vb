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
    Dim Threshold As Integer            'スレショルド電圧設定
    Dim Channel As Integer              'チャネル番号設定
    Dim FreeFlag As Integer             'フリーランモードフラグ

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
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents HScrollBar1 As System.Windows.Forms.HScrollBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents HScrollBar2 As System.Windows.Forms.HScrollBar
    Friend WithEvents HScrollBar3 As System.Windows.Forms.HScrollBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.HScrollBar1 = New System.Windows.Forms.HScrollBar
        Me.Label1 = New System.Windows.Forms.Label
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.HScrollBar2 = New System.Windows.Forms.HScrollBar
        Me.HScrollBar3 = New System.Windows.Forms.HScrollBar
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button5 = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(32, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 23)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "COM9"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.Location = New System.Drawing.Point(88, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(112, 40)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "USB接続"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(208, 16)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(72, 23)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = ""
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.Location = New System.Drawing.Point(32, 64)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(112, 40)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "手動計測"
        '
        'Timer1
        '
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button3.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.Location = New System.Drawing.Point(312, 8)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(112, 40)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "終了"
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button4.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.Location = New System.Drawing.Point(168, 64)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(112, 40)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "自動計測"
        '
        'HScrollBar1
        '
        Me.HScrollBar1.Location = New System.Drawing.Point(120, 120)
        Me.HScrollBar1.Name = "HScrollBar1"
        Me.HScrollBar1.Size = New System.Drawing.Size(160, 24)
        Me.HScrollBar1.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 24)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "水平軸時間"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Timer2
        '
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 160)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 24)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "同期レベル"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'HScrollBar2
        '
        Me.HScrollBar2.Location = New System.Drawing.Point(120, 160)
        Me.HScrollBar2.Name = "HScrollBar2"
        Me.HScrollBar2.Size = New System.Drawing.Size(160, 24)
        Me.HScrollBar2.TabIndex = 12
        '
        'HScrollBar3
        '
        Me.HScrollBar3.Location = New System.Drawing.Point(120, 200)
        Me.HScrollBar3.Name = "HScrollBar3"
        Me.HScrollBar3.Size = New System.Drawing.Size(160, 24)
        Me.HScrollBar3.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 200)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 24)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "サンプル周期"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button5.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Button5.Location = New System.Drawing.Point(312, 64)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(112, 40)
        Me.Button5.TabIndex = 15
        Me.Button5.Text = "フリーラン"
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Gainsboro
        Me.GroupBox1.Controls.Add(Me.RadioButton2)
        Me.GroupBox1.Controls.Add(Me.RadioButton1)
        Me.GroupBox1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(312, 128)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(112, 96)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "チャネル指定"
        '
        'RadioButton2
        '
        Me.RadioButton2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.RadioButton2.Location = New System.Drawing.Point(16, 56)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(88, 24)
        Me.RadioButton2.TabIndex = 1
        Me.RadioButton2.Text = "CH1(AC)"
        '
        'RadioButton1
        '
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.RadioButton1.Location = New System.Drawing.Point(16, 24)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(88, 24)
        Me.RadioButton1.TabIndex = 0
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "CH0(DC)"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(192, Byte), CType(255, Byte), CType(255, Byte))
        Me.ClientSize = New System.Drawing.Size(448, 246)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.HScrollBar3)
        Me.Controls.Add(Me.HScrollBar2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.HScrollBar1)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "オシロスコープ"
        Me.GroupBox1.ResumeLayout(False)
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

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        F1 = Me                 'フォームの名前設定
        '表示ピッチ
        HScrollBar1.Minimum = 1
        HScrollBar1.Maximum = 10
        HScrollBar1.SmallChange = 1
        HScrollBar1.LargeChange = 1
        HScrollBar1.Value = 1
        HDIV = 1
        '同期スレショルド
        HScrollBar2.Minimum = 1
        HScrollBar2.Maximum = 255
        HScrollBar2.SmallChange = 1
        HScrollBar2.LargeChange = 10
        Threshold = 90
        HScrollBar2.Value() = 90
        'サンプリング周期
        HScrollBar3.Minimum = 1
        HScrollBar3.Maximum = 100
        HScrollBar3.SmallChange = 1
        HScrollBar3.LargeChange = 10
        HScrollBar3.Value = 5
        Sample = 5
        'チャネル番号指定
        If RadioButton1.Checked = True Then Channel = 0
        If RadioButton1.Checked = True Then Channel = 1
        TextBox2.Text = ""
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
                        F2.Show()
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

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '測定開始
        Index = 0
        Timer2.Stop()
        Application.DoEvents()
        '測定開始コマンド
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H31) + Chr(Threshold) + Chr(Sample) + Chr(Channel)
        dLen = Len(wDATA)
        'コマンド送信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)

        '繰り返し計測周期をタイマに設定と開始
        Application.DoEvents()
        Timer1.Interval() = 500
        Timer1.Start()
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        '計測完了問い合わせと繰り返し測定
        Dim N, K As Integer
        Application.DoEvents()
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H32)                           '完了問い合わせコマンド
        dLen = Len(wDATA)
        'コマンド送信、折り返しデータ受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True Then
            'データ収集完了か確認
            If rDATA(0) = &H31 Then                 '１なら完了
                Timer1.Stop()                       '問い合わせ用タイマ停止
                For K = 0 To 14 Step 1              '15ブロックデータ収集
                    wDATA = Chr(&H33)               '計測データ取得コマンド
                    dLen = Len(wDATA)
                    'コマンド送信、折り返しデータ受信
                    bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
                    bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
                    If bRet = True Then
                        '64バイトのデータをバッファ保存
                        For N = 0 To rLen - 1 Step 1
                            mDATA(Index) = rDATA(N)
                            Index = Index + 1       'インデックスは連続
                        Next
                    End If
                Next K
                'グラフ表示指示　別フォームオープン
                F2.Activate()
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        '終了処理
        Timer1.Stop()
        Timer2.Stop()
        CloseHandle(0)
        F2.Close()
        Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        '自動計測　タイマ２で繰り返す
        Application.DoEvents()
        FreeFlag = &H31
        Timer2.Interval = 2000
        Timer2.Start()
        Timer2.Enabled = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'フリーランモード測定開始()
        Application.DoEvents()
        FreeFlag = &H34
        Timer2.Interval = 2000
        Timer2.Start()
        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        '自動計測の場合の繰り返し処理
        Index = 0
        F1.Activate()                                           'グラフ再表示のためF1にフォーカス
        Application.DoEvents()
        If RadioButton1.Checked = True Then Channel = 0
        If RadioButton2.Checked = True Then Channel = 1
        '測定開始コマンド
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(FreeFlag) + Chr(Threshold) + Chr(Sample) + Chr(Channel)
        dLen = Len(wDATA)                                       'スレショルドとサンプル周期も送信
        'コマンド送信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        '完了問い合わせ用タイマ開始
        Timer1.Interval() = 500
        Timer1.Start()
        Timer1.Enabled = True
    End Sub

    Private Sub HScrollBar1_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar1.Scroll
        '水平時間軸パラメータ取得
        HDIV = HScrollBar1.Value

    End Sub

    Private Sub HScrollBar2_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar2.Scroll
        'トリガ用スレッショルドパラメータ取得
        Threshold = HScrollBar2.Value
    End Sub

    Private Sub HScrollBar3_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles HScrollBar3.Scroll
        'サンプル周期パラメータ取得
        Sample = HScrollBar3.Value
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then Channel = 0
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then Channel = 1
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter
        If RadioButton1.Checked Then Channel = 0
        If RadioButton2.Checked Then Channel = 1
    End Sub
End Class

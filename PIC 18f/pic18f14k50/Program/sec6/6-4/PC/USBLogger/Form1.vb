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
    Dim Flag As Integer                 'データ保存終了フラグ
    Dim mDATA(10000) As Integer         'データ保存用配列
    Dim Index As Integer                '配列用インデックス


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
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.Button5 = New System.Windows.Forms.Button
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Button6 = New System.Windows.Forms.Button
        Me.Button7 = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button8 = New System.Windows.Forms.Button
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.Button9 = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(16, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(48, 23)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "COM6"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.SteelBlue
        Me.Button1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button1.Location = New System.Drawing.Point(80, 8)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(88, 32)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "USB接続"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(192, 16)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(64, 23)
        Me.TextBox2.TabIndex = 2
        Me.TextBox2.Text = "Fault"
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.SteelBlue
        Me.Button2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button2.Location = New System.Drawing.Point(352, 8)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 32)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "終了"
        '
        'TextBox3
        '
        Me.TextBox3.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(16, 184)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(448, 72)
        Me.TextBox3.TabIndex = 4
        Me.TextBox3.Text = "Data"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(16, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "計測データ表示/保存"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.SteelBlue
        Me.Button3.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button3.Location = New System.Drawing.Point(16, 136)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(104, 32)
        Me.Button3.TabIndex = 6
        Me.Button3.Text = "データ表示"
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.SteelBlue
        Me.Button4.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button4.Location = New System.Drawing.Point(184, 136)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(104, 32)
        Me.Button4.TabIndex = 7
        Me.Button4.Text = "データ保存"
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.SteelBlue
        Me.Button5.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button5.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button5.Location = New System.Drawing.Point(352, 136)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(104, 32)
        Me.Button5.TabIndex = 8
        Me.Button5.Text = "初期化"
        '
        'TextBox4
        '
        Me.TextBox4.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(120, 80)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(32, 23)
        Me.TextBox4.TabIndex = 9
        Me.TextBox4.Text = "10"
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.SteelBlue
        Me.Button6.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button6.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button6.Location = New System.Drawing.Point(16, 72)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(96, 32)
        Me.Button6.TabIndex = 10
        Me.Button6.Text = "周期設定"
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.Color.SteelBlue
        Me.Button7.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button7.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button7.Location = New System.Drawing.Point(184, 72)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(104, 32)
        Me.Button7.TabIndex = 11
        Me.Button7.Text = "測定開始"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "データログ制御"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(16, 264)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 16)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "直接表示制御"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.Color.SteelBlue
        Me.Button8.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button8.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.Button8.Location = New System.Drawing.Point(16, 280)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(104, 32)
        Me.Button8.TabIndex = 14
        Me.Button8.Text = "データ要求"
        '
        'TextBox5
        '
        Me.TextBox5.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(152, 288)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(240, 23)
        Me.TextBox5.TabIndex = 15
        Me.TextBox5.Text = "Data"
        Me.TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.Color.FromArgb(CType(255, Byte), CType(192, Byte), CType(192, Byte))
        Me.Button9.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Button9.Location = New System.Drawing.Point(352, 72)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(104, 32)
        Me.Button9.TabIndex = 16
        Me.Button9.Text = "EEPROM消去"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(376, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 16)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "実行時間長い"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 12)
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(480, 326)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "Data Logger"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TimeOutSet()
        'COM通信タイムアウト用構造体データ設定
        timeOut.ReadIntervalTimeout = 10
        timeOut.ReadTotalTimeoutConstant = 30
        timeOut.ReadTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutMultiplier = 10
        timeOut.WriteTotalTimeoutConstant = 30
        bRet = SetCommTimeouts(hComm, timeOut)
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'USB接続制御
        Dim N As Integer
        CommName = TextBox1.Text            'COMポート番号取得
        'COMポートオープン
        bRet = CloseHandle(hComm)
        hComm = CreateFile(CommName, &HC0000000, 0, IntPtr.Zero, &H3, &H80, IntPtr.Zero)
        If hComm <> -1 Then
            'DCBデータ設定
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
        '初期化
        TextBox2.Text = ""
        CloseHandle(0)
        Index = 0
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        '終了処理
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'ログデータ読み取り、表示処理
        Dim N As Integer
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H33)           'コマンド
        dLen = Len(wDATA)
        'コマンド送信後　折り返し32個のデータ取得
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True Then
            '32個のデータを10進数で表示
            For N = 0 To rLen - 1 Step 2
                If (N > 16) And (N Mod 16 = 0) Then
                    TextBox3.Text = TextBox3.Text & vbCrLf
                End If
                TextBox3.Text = TextBox3.Text & Format(rDATA(N) * 256 + rDATA(N + 1), "00000") & "  "
            Next
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'データ保存処理
        Dim N As Integer
        Dim svFile1 As New SaveFileDialog
        '初期値設定
        TextBox3.Text = ""
        Index = 0
        Flag = False
        'データ取得
        While Flag = False
            bRet = SetCommState(hComm, stDCB)
            wDATA = Chr(&H33)       'コマンド
            dLen = Len(wDATA)
            'コマンド送信後　64バイトのデータ取得
            bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
            bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
            If bRet = True Then
                'データを２バイト毎に配列にバイナリで保存
                For N = 0 To rLen - 1 Step 2
                    mDATA(Index) = rDATA(N) * 256 + rDATA(N + 1)
                    If mDATA(Index) = &HFFFF Then
                        Flag = True
                        Exit For
                    End If
                    Index = Index + 1
                Next
            End If
        End While
        '取得完了ファイル保存
        TextBox3.Text = "データ取得完了"
        bRet = MessageBox.Show("ファイルに保存します。")
        'ファイルダイアログからファイル名取得
        svFile1.Filter = "data file(*.dat)|*dat|all file(*.*)|*.*"
        svFile1.FilterIndex = 1
        svFile1.RestoreDirectory = True
        Dim ret As DialogResult = svFile1.ShowDialog
        If ret <> DialogResult.Cancel Then
            'ファイル保存実行
            FileOpen(1, svFile1.FileName, OpenMode.Output)
            For N = 0 To Index
                Write(1, mDATA(N))
            Next
            FileClose(1)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'EEPROMアドレス初期化
        TextBox3.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H30)       'コマンド
        dLen = Len(wDATA)
        'コマンド送信後折り返しデータ確認
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True And rLen > 1 Then
            '初期化正常完了
            TextBox3.Text = "初期化完了"
            Button7.Text = "測定開始"
        Else
            '初期化異常終了
            TextBox3.Text = "初期化失敗"
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        'ログ周期設定
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H31) & Chr(Val(TextBox4.Text))     '周期データ取得
        dLen = Len(wDATA)
        'コマンド送信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        'データログ開始、停止制御
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H37)           'コマンド
        dLen = Len(wDATA)
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        '状態更新、開始、停止交互制御
        If Button7.Text = "測定停止" Then
            Button7.Text = "測定開始"
        Else
            Button7.Text = "測定停止"
        End If
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        '個別計測要求
        Dim N As Integer
        TextBox5.Text = ""
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H32)       'コマンド
        dLen = Len(wDATA)
        'コマンド送信後　折り返しの計測データ受信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        bRet = ReadFile(hComm, rDATA, 64, rLen, IntPtr.Zero)
        If bRet = True Then
            '計測データ表示　３チャネル同時
            For N = 0 To rLen - 1
                TextBox5.Text = TextBox5.Text & Chr(rDATA(N))
            Next
        End If
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        'EEPROM消去
        bRet = SetCommState(hComm, stDCB)
        wDATA = Chr(&H36)
        dLen = Len(wDATA)
        'コマンド送信
        bRet = WriteFile(hComm, wDATA, dLen, wLen, IntPtr.Zero)
        If bRet = True Then
            Close()
        End If
    End Sub
End Class

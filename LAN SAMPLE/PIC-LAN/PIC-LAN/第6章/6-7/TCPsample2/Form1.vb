'Socketクラスの呼び出し
Imports System
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1
    'Socketクラス インスタンス生成
    Dim TcpClientA As New TcpClient()
    Dim MyStream As NetworkStream

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TCPリモート接続
        Try
            TcpClientA.Connect(TextBox1.Text, Integer.Parse(TextBox2.Text))
            TextBox3.Text = "Connect!"
            MyStream = TcpClientA.GetStream()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Timer1.Enabled = False      'タイマ１停止
        MyStream.Close()
        TcpClientA.Close()          'TCPのクローズ
        Close()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = False
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'コマンドデータ送信
        Me.Enabled = False
        Try
            '現在状態によりオンオフを反転制御
            If Button3.BackColor = Color.Red Then
                MyStream.Write(LED1Off, 0, LED1Off.Length)
            Else
                MyStream.Write(LED1On, 0, LED1On.Length)
            End If
            '折り返しデータ受信（MLXXE)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'オンオフ状態により色とテキストを変更
            If RcvBuf(1) = &H4C And RcvBuf(2) = &H31 Then
                Button3.BackColor = Color.Red
            End If
            If RcvBuf(1) = &H4C And RcvBuf(2) = &H30 Then
                Button3.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'コマンドデータ送信
        Me.Enabled = False
        Try
            '現在状態によりオンオフを反転制御
            If Button4.BackColor = Color.Red Then
                MyStream.Write(LED2Off, 0, LED2Off.Length)
            Else
                MyStream.Write(LED2On, 0, LED2On.Length)
            End If

            '折り返しデータ受信（MLXXE)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'オンオフ状態により色とテキストを変更
            If RcvBuf(1) = &H4C And RcvBuf(3) = &H31 Then
                Button4.BackColor = Color.Red
            End If
            If RcvBuf(1) = &H4C And RcvBuf(3) = &H30 Then
                Button4.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Me.Enabled = False
        Try
            'スイッチ状態入力コマンド送信
            MyStream.Write(SWInput, 0, SWInput.Length)

            '折り返し受信（MBXXXX)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'Xが0か1でオンオフ表示制御
            If RcvBuf(2) = &H30 Then
                TextBox4.BackColor = Color.Red
            Else
                TextBox4.BackColor = Color.Green
            End If
            If RcvBuf(3) = &H30 Then
                TextBox5.BackColor = Color.Red
            Else
                TextBox5.BackColor = Color.Green
            End If
            If RcvBuf(4) = &H30 Then
                TextBox6.BackColor = Color.Red
            Else
                TextBox6.BackColor = Color.Green
            End If
            If RcvBuf(5) = &H30 Then
                TextBox7.BackColor = Color.Red
            Else
                TextBox7.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If Timer1.Enabled = False Then
            Timer1.Interval() = 500     '500msecタイマ起動
            Timer1.Enabled() = True     'タイマ１起動
            Button6.Text = "停止"
        Else
            Timer1.Enabled = False
            Button6.Text = "計測"

        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Enabled = False
        Try
            'チャネル1計測
            MyStream.Write(AN2Mesure, 0, AN2Mesure.Length)
            '折り返しデータ受信（MAXXXX)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'データ変換と表示
            RcvBuf(0) = &H20              'Mをスペースに変更
            RcvBuf(1) = &H20              'Aをスペースに変更
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox8.Text = returnData.ToString()
            Application.DoEvents()        'テキストボックス表示可能化
            'チャネル2計測
            MyStream.Write(AN3Mesure, 0, AN3Mesure.Length)
            '折り返しデータ受信（MAXXXX)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '計測データ変換と表示
            RcvBuf(0) = &H20              'Mをスペースに変更
            RcvBuf(1) = &H20              'Aをスペースに変更
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox9.Text = returnData.ToString()
            Application.DoEvents()        'テキストボックス表示可能化
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Me.Enabled = False
        Try
            '液晶表示器への表示制御
            Dim SendMessage As Byte()

            SendMessage = Encoding.ASCII.GetBytes("SD" + TextBox10.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '折り返しデータ受信（M?)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'データ変換と表示
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Me.Enabled = False
        Try
            '液晶表示器の消去
            MyStream.Write(LCDErase, 0, LCDErase.Length)
            '折り返しデータ受信（M?)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'データ変換と表示
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox3.Text = returnData.ToString()
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    '共通変数の定義
    Dim receiveBytes As Byte()
    Dim returnData As String
    Dim RcvBuf(200) As Byte
    '送信コマンドデータ定義
    Dim LED1On As Byte() = Encoding.ASCII.GetBytes("SC11")
    Dim LED1Off As Byte() = Encoding.ASCII.GetBytes("SC10")
    Dim LED2On As Byte() = Encoding.ASCII.GetBytes("SC21")
    Dim LED2Off As Byte() = Encoding.ASCII.GetBytes("SC20")
    Dim AN2Mesure As Byte() = Encoding.ASCII.GetBytes("SA1")
    Dim AN3Mesure As Byte() = Encoding.ASCII.GetBytes("SA2")
    Dim SWInput As Byte() = Encoding.ASCII.GetBytes("SB")
    Dim LCDErase As Byte() = Encoding.ASCII.GetBytes("SE")
End Class

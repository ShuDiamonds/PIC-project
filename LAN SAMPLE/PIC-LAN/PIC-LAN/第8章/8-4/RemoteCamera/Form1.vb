'Socketクラスの呼び出し
Imports System
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports Microsoft.VisualBasic

Public Class Form1
    'Socketクラス インスタンス生成
    Dim TcpClientA As New TcpClient()
    Dim MyStream As NetworkStream
    Dim Flag As Integer
    Dim RcvBuf(200) As Byte

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            TcpClientA.Connect(TextBox1.Text, Integer.Parse(TextBox6.Text))
            TextBox2.Text = "Connect!"
            MyStream = TcpClientA.GetStream()
            Flag = 0
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Timer1.Enabled = False      'タイマ１停止
        TcpClientA.Close()          'TCPのクローズ
        MyStream.Close()
        Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'コマンドデータ送信
        Me.Enabled = False
        Try
            '現在状態によりオンオフを反転制御
            If Button3.BackColor = Color.Red Then
                MyStream.Write(UIO1Off, 0, UIO1Off.Length)
            Else
                MyStream.Write(UIO1On, 0, UIO1On.Length)
            End If
            '折り返しデータ受信（MLXXE)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'オンオフ状態により色を変更
            If RcvBuf(1) = &H4C And RcvBuf(2) = &H31 Then
                Button3.BackColor = Color.Red
            End If
            If RcvBuf(1) = &H4C And RcvBuf(2) = &H30 Then
                Button3.BackColor = Color.Green
            End If
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'コマンドデータ送信
        Me.Enabled = False
        Try
            '現在状態によりオンオフを反転制御
            If Button4.BackColor = Color.Red Then
                MyStream.Write(UIO2Off, 0, UIO2Off.Length)
            Else
                MyStream.Write(UIO2On, 0, UIO2On.Length)
            End If

            '折り返しデータ受信（MLXXE)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'オンオフ状態により色を変更
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
        If Timer1.Enabled = False Then
            Timer1.Interval() = 500     '500msecタイマ起動
            Timer1.Enabled() = True     'タイマ１起動
            Button5.Text = "停止"
        Else
            Timer1.Enabled = False
            Button5.Text = "計測"

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
            TextBox3.Text = returnData.ToString()
            Application.DoEvents()        'テキストボックス表示可能化
            'チャネル2計測
            MyStream.Write(AN3Mesure, 0, AN3Mesure.Length)
            '折り返しデータ受信（MAXXXX)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            '計測データ変換と表示
            RcvBuf(0) = &H20              'Mをスペースに変更
            RcvBuf(1) = &H20              'Aをスペースに変更
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox4.Text = returnData.ToString()
            Application.DoEvents()        'テキストボックス表示可能化
        Catch ex As Exception
            TextBox3.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Me.Enabled = False
        Try
            '液晶表示器への表示制御
            Dim SendMessage As Byte()

            SendMessage = Encoding.ASCII.GetBytes("SD" + TextBox5.Text + vbNullChar)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
            '折り返しデータ受信（M?)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'データ変換と表示
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox2.Text = returnData.ToString()
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Me.Enabled = False
        Try
            '液晶表示器の消去
            MyStream.Write(LCDErase, 0, LCDErase.Length)
            '折り返しデータ受信（M?)
            MyStream.Read(RcvBuf, 0, RcvBuf.Length)
            'データ変換と表示
            returnData = Encoding.ASCII.GetString(RcvBuf)
            TextBox2.Text = returnData.ToString()
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
        Me.Enabled = True
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        Dim HorStr As String
        Dim SendMessage As Byte()

        TrackBar1.TickFrequency = 100
        Try
            HorStr = "SR1" + Str(TrackBar1.Value) + vbNullChar
            SendMessage = Encoding.ASCII.GetBytes(HorStr)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
    End Sub

    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar2.Scroll
        Dim VarStr As String
        Dim SendMessage As Byte()

        TrackBar2.TickFrequency = 100
        Try
            '上下の値を反転して送信（中心値1950)
            VarStr = "SR2" + Str(1950 - (TrackBar2.Value - 1950)) + vbNullChar
            SendMessage = Encoding.ASCII.GetBytes(VarStr)
            MyStream.Write(SendMessage, 0, SendMessage.Length)
        Catch ex As Exception
            TextBox2.Text = ex.ToString()
        End Try
    End Sub
    '共通変数の定義
    Dim receiveBytes As Byte()
    Dim returnData As String
    '送信コマンドデータ定義
    Dim UIO1On As Byte() = Encoding.ASCII.GetBytes("SC11")
    Dim UIO1Off As Byte() = Encoding.ASCII.GetBytes("SC10")
    Dim UIO2On As Byte() = Encoding.ASCII.GetBytes("SC21")
    Dim UIO2Off As Byte() = Encoding.ASCII.GetBytes("SC20")
    Dim AN2Mesure As Byte() = Encoding.ASCII.GetBytes("SA1")
    Dim AN3Mesure As Byte() = Encoding.ASCII.GetBytes("SA2")
    Dim LCDErase As Byte() = Encoding.ASCII.GetBytes("SE")
    Dim HSlider As Byte() = Encoding.ASCII.GetBytes("SR1")
    Dim VSlider As Byte() = Encoding.ASCII.GetBytes("SR2")

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class


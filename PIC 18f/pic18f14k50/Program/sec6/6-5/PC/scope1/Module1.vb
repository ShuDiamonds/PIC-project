Module Module1
    Public Structure DCB
        'DCB構造体の定義
        Public DCBlength As Int32
        Public BaudRate As Int32
        Public fBitFields As Int32
        Public wReserved As Int16
        Public XonLim As Int16
        Public XoffLim As Int16
        Public ByteSize As Byte
        Public Parity As Byte
        Public StopBits As Byte
        Public XonChar As Byte
        Public XoffChar As Byte
        Public ErrorChar As Byte
        Public EofChar As Byte
        Public EvtChar As Byte
    End Structure

    Public Structure COMMTIMEOUTS
        'タイムアウト用構造体の定義
        Public ReadIntervalTimeout As Int32
        Public ReadTotalTimeoutMultiplier As Int32
        Public ReadTotalTimeoutConstant As Int32
        Public WriteTotalTimeoutMultiplier As Int32
        Public WriteTotalTimeoutConstant As Int32
    End Structure

    'COM用関数の宣言
    Public Declare Function CreateFile Lib "kernel32.dll" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Int32, ByVal dwShareMode As Int32, ByVal lpSecurityAttributes As IntPtr, ByVal dwCreatonDisposition As Int32, ByVal dwFlagsAndAttributes As Int32, ByVal hTemplateFile As IntPtr) As Integer
    Public Declare Function CloseHandle Lib "kernel32.dll" (ByVal hComm As Integer) As Boolean
    Public Declare Function SetCommState Lib "kernel32.dll" (ByVal hComm As Integer, ByRef lpDCB As DCB) As Boolean
    Public Declare Function WriteFile Lib "kernel32.dll" (ByVal hComm As Integer, ByVal lpBuffer As String, ByVal nNumberOfBytesToWrite As Int32, ByRef lpNumberOfBytesWritten As Int32, ByVal lpOverlapped As IntPtr) As Boolean
    Public Declare Function ReadFile Lib "kernel32.dll" (ByVal hComm As Integer, ByVal lpBuffer As Byte(), ByVal nNumberOfBytesToRead As Int32, ByRef lpNumberOfBytesRead As Int32, ByVal lpOverlapped As IntPtr) As Boolean
    Public Declare Function SetCommTimeouts Lib "kernel32.dll" (ByVal hComm As Integer, ByRef lpCommTimeouts As COMMTIMEOUTS) As Boolean
    Public Declare Function GetCommState Lib "kernel32.dll" (ByVal hComm As Integer, ByRef lpDCB As DCB) As Boolean

    'フォームのインスタンス生成
    Public F1 As New Form1
    Public F2 As New Form2
    '共通関数宣言
    Public mDATA(1000) As Integer        'データ保存用配列
    Public Index As Integer             '配列用インデックス
    Public HDIV As Integer              '水平時間軸パラメータ
    Public Sample As Integer            'サンプル周期パラメータ


End Module

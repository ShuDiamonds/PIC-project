Attribute VB_Name = "Module1"
'USB�h���C�oDLL�t�@���N�V����
Declare Function Uusbd_Open Lib "UUSBD.DLL" () As Long
Declare Function Uusbd_Open_mask Lib "UUSBD.DLL" (ByVal flag As Long, ByVal Class As Byte, ByVal SubClass As Byte, ByVal Vendor As Integer, ByVal Product As Integer, ByVal bcdDevice As Byte) As Long
Declare Function Uusbd_Close Lib "UUSBD.DLL" (ByVal hUSB As Long) As Long
Declare Function Uusbd_OpenPipe Lib "UUSBD.DLL" (ByVal hUSB As Long, ByVal Interface_num As Byte, ByVal pipe_num As Byte) As Long
Declare Function Uusbd_ResetPipe Lib "UUSBD.DLL" (ByVal hFile As Long) As Long
Declare Function Uusbd_ResetDevice Lib "UUSBD.DLL" (ByVal hUSB As Long) As Long
Declare Function Uusbd_ClassRequest Lib "UUSBD.DLL" (ByVal hUSB As Long, ByVal dir_in As Long, ByVal recipient As Byte, ByVal bRequest As Byte, ByVal wValue As Integer, ByVal wIndex As Integer, ByVal wLength As Integer, ByVal data As Long) As Long
Declare Function Uusbd_VendorRequest Lib "UUSBD.DLL" (ByVal hUSB As Long, ByVal dir_in As Long, ByVal recipient As Byte, ByVal bRequest As Byte, ByVal wValue As Integer, ByVal wIndex As Integer, ByVal wLength As Integer, ByVal data As Long) As Long

'�t�@�C���n���h�����o��
Declare Function WriteFile Lib "kernel32" (ByVal hFile As Long, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToWrite As Long, ByRef lpNumberOfBytesWritten As Long, ByVal lpOverlapped As Long) As Long
Declare Function ReadFile Lib "kernel32" (ByVal hFile As Long, ByRef lpBuffer As Byte, ByVal nNumberOfBytesToRead As Long, ByRef lpNumberOfBytesRead As Long, ByVal lpOverlapped As Long) As Long
Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long

'�e��n���h��
Global hUSB As Long         'USB�n���h��
Global hCMD As Long         '�R�}���h�o��
Global hSTA As Long         '�f�[�^����
Global hOUT As Long         '�ėp�o��
Global hINP As Long         '�ėp����

'�I�[�v������USB�^�[�Q�b�g�f�o�C�X�̃x���_ID�ƃv���_�N�gID
Global Const Vendor = &H9B9     '�x���_ID
Global Const Product = &H48   '�v���_�N�gID

'�}�X�N�����ݒ�r�b�g
Global Const UU_MASK_NO = 0
Global Const UU_MASK_CLASS = 1
Global Const UU_MASK_SUBCLASS = 2
Global Const UU_MASK_VENDOR = 4
Global Const UU_MASK_PRODUCT = 8
Global Const UU_MASK_BCDDEVICE = 16


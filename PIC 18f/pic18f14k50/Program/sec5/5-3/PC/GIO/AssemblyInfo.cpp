#include "stdafx.h"

using namespace System::Reflection;
using namespace System::Runtime::CompilerServices;

//
// �A�Z���u���Ɋւ����ʏ��͈ȉ��� 
// �����Z�b�g��ʂ��Đ��䂳��܂��B�A�Z���u���Ɋ֘A�t�����Ă��� 
// ����ύX����ɂ́A�����̑����l��ύX���Ă��������B
//
[assembly:AssemblyTitleAttribute("")];
[assembly:AssemblyDescriptionAttribute("")];
[assembly:AssemblyConfigurationAttribute("")];
[assembly:AssemblyCompanyAttribute("")];
[assembly:AssemblyProductAttribute("")];
[assembly:AssemblyCopyrightAttribute("")];
[assembly:AssemblyTrademarkAttribute("")];
[assembly:AssemblyCultureAttribute("")];		

//
// �A�Z���u���̃o�[�W�������́A�ȉ��� 4 �̑����ō\������܂� :
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// ���ɂ���悤�ɁA'*' ���g���āA���ׂĂ̒l���w�肷�邩�A
// �r���h����у��r�W�����ԍ�������l�ɂ��邱�Ƃ��ł��܂��B

[assembly:AssemblyVersionAttribute("1.0.*")];

//
// �A�Z���u���ɏ�������ɂ́A�g�p����L�[���w�肵�Ȃ���΂Ȃ�܂���B�A�Z���u�������Ɋւ��� 
// �ڍׂɂ��ẮAMicrosoft .NET Framework �h�L�������g���Q�Ƃ��Ă��������B
//
// ���̑������g���āA�����Ɏg���L�[�𐧌䂵�܂��B
//
// ���� : 
//   (*) �L�[���w�肳��Ȃ��ƁA�A�Z���u���͏�������܂���B
//   (*) KeyName �́A�R���s���[�^�ɃC���X�g�[������Ă���Í��T�[�r�X �v���o�C�_ (CSP) ��
//       �\���܂��BKeyFile �́A�L�[���܂�
//       �t�@�C���ł��B
//   (*) KeyFile ����� KeyName �̒l�����Ɏw�肳��Ă���ꍇ�A 
//       �ȉ��̏������������܂� :
//       (1) KeyName �� CSP �Ɍ��������ꍇ�A���̃L�[���g���܂��B
//       (2) KeyName �����݂����AKeyFile �����݂���ꍇ�A
//           KeyFile �ɂ���L�[�� CSP �ɃC���X�g�[������A�g���܂��B
//   (*) KeyFile ���쐬����ɂ́Asn.exe (Strong Name) ���[�e�B���e�B���g���Ă��������B
//        KeyFile ���w�肷��Ƃ��AKeyFile �̏ꏊ�́A
//        �v���W�F�N�g�o�� �f�B���N�g���ւ̑��΃p�X�łȂ���΂Ȃ�܂���B
//   (*) �x�������͍��x�ȃI�v�V�����ł��B�ڍׂɂ��Ă� Microsoft .NET Framework
//       �h�L�������g���Q�Ƃ��Ă��������B
//
[assembly:AssemblyDelaySignAttribute(false)];
[assembly:AssemblyKeyFileAttribute("")];
[assembly:AssemblyKeyNameAttribute("")];


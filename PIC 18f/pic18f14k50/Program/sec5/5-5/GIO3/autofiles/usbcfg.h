/*********************************************************************
 *
 *                Microchip USB C18 Firmware Version 1.0
 *
 *********************************************************************
 * FileName:        usbcfg.h
 * Dependencies:    See INCLUDES section below
 * Processor:       PIC18
 * Compiler:        C18 2.30.01+
 * Company:         Microchip Technology, Inc.
 *
 * Software License Agreement
 *
 * The software supplied herewith by Microchip Technology Incorporated
 * (the �Company�E for its PICmicro� Microcontroller is intended and
 * supplied to you, the Company�s customer, for use solely and
 * exclusively on Microchip PICmicro Microcontroller products. The
 * software is owned by the Company and/or its supplier, and is
 * protected under applicable copyright laws. All rights are reserved.
 * Any use in violation of the foregoing restrictions may subject the
 * user to criminal sanctions under applicable laws, as well as to
 * civil liability for the breach of the terms and conditions of this
 * license.
 *
 * THIS SOFTWARE IS PROVIDED IN AN �AS IS�ECONDITION. NO WARRANTIES,
 * WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED
 * TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,
 * IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR
 * CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.
 *
 ********************************************************************/

#ifndef USBCFG_H
#define USBCFG_H

/** D E F I N I T I O N S *******************************************/
#define EP0_BUFF_SIZE           8   // 8, 16, 32, or 64
#define MAX_NUM_INT             1   // For tracking Alternate Setting

/* Parameter definitions are defined in usbdrv.h */
#define MODE_PP                 _PPBM0
#define UCFG_VAL                _PUEN|_TRINT|_FS|MODE_PP

//#define USE_SELF_POWER_SENSE_IO
//#define USE_USB_BUS_SENSE_IO

/** D E V I C E  C L A S S  U S A G E *******************************/

#define USB_USE_GEN

/*
 * MUID = Microchip USB Class ID
 * Used to identify which of the USB classes owns the current
 * session of control transfer over EP0
 */
#define MUID_NULL               0
#define MUID_USB9               1

/** E N D P O I N T S  A L L O C A T I O N **************************/
/*
 * See usbmmap.c for an explanation of how the endpoint allocation works
 */

/* PICDEM FS USB Demo (using generic usb class template) */
#define USBGEN_INTF_ID          0x00
#define USBGEN_UEP              UEP1
#define USBGEN_BD_OUT           ep1Bo
#define USBGEN_BD_IN            ep1Bi
#define USBGEN_EP_SIZE          64

/// �ǉ��C���^�[�t�F�[�X�p�ǉ�
#define USBIO_INTF_ID          	0x01
#define USBIO_UEP2			UEP2
#define USBIO_IO_OUT    	    	ep2Bo
#define USBIO_IO_IN            	ep2Bi
#define USBIO_EP_SIZE			32

#define MAX_EP_NUMBER           2           // UEP2����ǉ�

#endif //USBCFG_H

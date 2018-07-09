#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Include project Makefile
include Makefile

# Environment
MKDIR=mkdir -p
RM=rm -f 
CP=cp 
# Macros
CND_CONF=PIC24FJ64GB004_PIM

ifeq ($(TYPE_IMAGE), DEBUG_RUN)
IMAGE_TYPE=debug
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf
else
IMAGE_TYPE=production
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf
endif
# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}
# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Object Files
OBJECTFILES=${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o ${OBJECTDIR}/_ext/1472/main.o ${OBJECTDIR}/_ext/1472/usb_descriptors.o ${OBJECTDIR}/_ext/926206843/usb_device.o ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o


CFLAGS=
ASFLAGS=
LDLIBSOPTIONS=

# Path to java used to run MPLAB X when this makefile was created
MP_JAVA_PATH=C:\\Program\ Files\\Java\\jdk1.6.0_23\\jre/bin/
OS_ORIGINAL="MINGW32_NT-5.1"
OS_CURRENT="$(shell uname -s)"
############# Tool locations ##########################################
# If you copy a project from one host to another, the path where the  #
# compiler is installed may be different.                             #
# If you open this project with MPLAB X in the new host, this         #
# makefile will be regenerated and the paths will be corrected.       #
#######################################################################
MP_CC=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin\\pic30-gcc.exe
# MP_BC is not defined
MP_AS=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin\\pic30-as.exe
MP_LD=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin\\pic30-ld.exe
MP_AR=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin\\pic30-ar.exe
# MP_BC is not defined
MP_CC_DIR=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin
# MP_BC_DIR is not defined
MP_AS_DIR=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin
MP_LD_DIR=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin
MP_AR_DIR=C:\\Program\ Files\\Microchip\\MPLAB\ C30\\bin
# MP_BC_DIR is not defined
.build-conf: ${BUILD_SUBPROJECTS}
ifneq ($(OS_CURRENT),$(OS_ORIGINAL))
	@echo "***** WARNING: This make file contains OS dependent code. The OS this makefile is being run is different from the OS it was created in."
endif
	${MAKE}  -f nbproject/Makefile-PIC24FJ64GB004_PIM.mk dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf

MP_PROCESSOR_OPTION=24FJ64GB004
MP_LINKER_FILE_OPTION=,-Tp24FJ64GB004.gld
# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o: ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/1083301514 
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d -o ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${CP} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${CP} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp
endif
${OBJECTDIR}/_ext/1472/main.o: ../main.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/1472/main.o.d -o ${OBJECTDIR}/_ext/1472/main.o ../main.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/1472/main.o.d > ${OBJECTDIR}/_ext/1472/main.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/main.o.tmp ${OBJECTDIR}/_ext/1472/main.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/main.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/1472/main.o.d > ${OBJECTDIR}/_ext/1472/main.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/main.o.tmp ${OBJECTDIR}/_ext/1472/main.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/main.o.tmp
endif
${OBJECTDIR}/_ext/1472/usb_descriptors.o: ../usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d -o ${OBJECTDIR}/_ext/1472/usb_descriptors.o ../usb_descriptors.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d > ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d > ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp
endif
${OBJECTDIR}/_ext/926206843/usb_device.o: ../../../../Microchip/USB/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/926206843 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/926206843/usb_device.o.d -o ${OBJECTDIR}/_ext/926206843/usb_device.o ../../../../Microchip/USB/usb_device.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/926206843/usb_device.o.d > ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/926206843/usb_device.o.d > ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp
endif
${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o: ../../../../Microchip/USB/usb_hal_pic24.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/926206843 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE) -g -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d -o ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o ../../../../Microchip/USB/usb_hal_pic24.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d > ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d > ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp
endif
else
${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o: ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/1083301514 
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE)  -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d -o ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${CP} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${CP} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.tmp
endif
${OBJECTDIR}/_ext/1472/main.o: ../main.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE)  -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/1472/main.o.d -o ${OBJECTDIR}/_ext/1472/main.o ../main.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/1472/main.o.d > ${OBJECTDIR}/_ext/1472/main.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/main.o.tmp ${OBJECTDIR}/_ext/1472/main.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/main.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/1472/main.o.d > ${OBJECTDIR}/_ext/1472/main.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/main.o.tmp ${OBJECTDIR}/_ext/1472/main.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/main.o.tmp
endif
${OBJECTDIR}/_ext/1472/usb_descriptors.o: ../usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE)  -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d -o ${OBJECTDIR}/_ext/1472/usb_descriptors.o ../usb_descriptors.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d > ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d > ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${CP} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.tmp
endif
${OBJECTDIR}/_ext/926206843/usb_device.o: ../../../../Microchip/USB/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/926206843 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE)  -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/926206843/usb_device.o.d -o ${OBJECTDIR}/_ext/926206843/usb_device.o ../../../../Microchip/USB/usb_device.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/926206843/usb_device.o.d > ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/926206843/usb_device.o.d > ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.tmp
endif
${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o: ../../../../Microchip/USB/usb_hal_pic24.c  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} ${OBJECTDIR}/_ext/926206843 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${MP_CC} $(MP_EXTRA_CC_PRE)  -omf=elf -x c -c -mcpu=$(MP_PROCESSOR_OPTION) -fno-short-double -I".." -I"../../../../Microchip/Include" -MMD -MF ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d -o ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o ../../../../Microchip/USB/usb_hal_pic24.c  
ifneq (,$(findstring MINGW32,$(OS_CURRENT))) 
	@sed -e 's/\"//g' -e 's/\\$$/__EOL__/g' -e 's/\\ /__ESCAPED_SPACES__/g' -e 's/\\/\//g' -e 's/__ESCAPED_SPACES__/\\ /g' -e 's/__EOL__$$/\\/g' ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d > ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp}
else 
	@sed -e 's/\"//g' ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d > ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${CP} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.d 
	${RM} ${OBJECTDIR}/_ext/926206843/usb_hal_pic24.o.tmp
endif
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE)  -omf=elf  -mcpu=$(MP_PROCESSOR_OPTION)  -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -o dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf ${OBJECTFILES}        -Wl,--defsym=__MPLAB_BUILD=1,--report-mem$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION),--defsym=__MPLAB_DEBUG=1,--defsym=__ICD2RAM=1,--defsym=__DEBUG=1,--defsym=__MPLAB_DEBUGGER_REAL_ICE=1
else
dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE)  -omf=elf  -mcpu=$(MP_PROCESSOR_OPTION)  -o dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf ${OBJECTFILES}        -Wl,--defsym=__MPLAB_BUILD=1,--report-mem$(MP_EXTRA_LD_POST)$(MP_LINKER_FILE_OPTION)
	${MP_CC_DIR}\\pic30-bin2hex dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.elf -omf=elf
endif


# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/PIC24FJ64GB004_PIM
	${RM} -r dist/PIC24FJ64GB004_PIM

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc

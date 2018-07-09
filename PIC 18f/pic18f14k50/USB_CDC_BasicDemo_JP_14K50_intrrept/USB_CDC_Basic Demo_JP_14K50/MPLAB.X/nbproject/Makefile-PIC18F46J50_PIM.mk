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
CND_CONF=PIC18F46J50_PIM

ifeq ($(TYPE_IMAGE), DEBUG_RUN)
IMAGE_TYPE=debug
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.cof
else
IMAGE_TYPE=production
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.cof
endif
# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}
# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Object Files
OBJECTFILES=${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o ${OBJECTDIR}/_ext/1472/main.o ${OBJECTDIR}/_ext/1472/usb_descriptors.o ${OBJECTDIR}/_ext/926206843/usb_device.o


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
MP_CC=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin\\mcc18.exe
# MP_BC is not defined
MP_AS=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin\\..\\mpasm\\MPASMWIN.exe
MP_LD=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin\\mplink.exe
MP_AR=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin\\mplib.exe
# MP_BC is not defined
MP_CC_DIR=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin
# MP_BC_DIR is not defined
MP_AS_DIR=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin\\..\\mpasm
MP_LD_DIR=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin
MP_AR_DIR=C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin
# MP_BC_DIR is not defined
# This makefile will use a C preprocessor to generate dependency files
MP_CPP=C:/Program\ Files/Microchip/MPLABX/mplab_ide/mplab_ide/modules/../../bin/mplab-cpp
.build-conf: ${BUILD_SUBPROJECTS}
	${MAKE}  -f nbproject/Makefile-PIC18F46J50_PIM.mk dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.cof

MP_PROCESSOR_OPTION=18F46J50
MP_PROCESSOR_OPTION_LD=18f46j50
MP_LINKER_DEBUG_OPTION= -u_DEBUGCODESTART=0x3e00 -u_DEBUGCODELEN=0x200
# ------------------------------------------------------------------------------------
# Rules for buildStep: createRevGrep
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
__revgrep__:   nbproject/Makefile-${CND_CONF}.mk
	@echo 'grep -q $$@' > __revgrep__
	@echo 'if [ "$$?" -ne "0" ]; then' >> __revgrep__
	@echo '  exit 0' >> __revgrep__
	@echo 'else' >> __revgrep__
	@echo '  exit 1' >> __revgrep__
	@echo 'fi' >> __revgrep__
	@chmod +x __revgrep__
else
__revgrep__:   nbproject/Makefile-${CND_CONF}.mk
	@echo 'grep -q $$@' > __revgrep__
	@echo 'if [ "$$?" -ne "0" ]; then' >> __revgrep__
	@echo '  exit 0' >> __revgrep__
	@echo 'else' >> __revgrep__
	@echo '  exit 1' >> __revgrep__
	@echo 'fi' >> __revgrep__
	@chmod +x __revgrep__
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o: ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/1083301514 
	${MP_CC} $(MP_EXTRA_CC_PRE) -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o   ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.temp ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/1083301514 > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d
else
	cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.temp >> ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d
endif
	${RM} __temp_cpp_output__
${OBJECTDIR}/_ext/1472/main.o: ../main.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${MP_CC} $(MP_EXTRA_CC_PRE) -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/1472/main.o   ../main.c  > ${OBJECTDIR}/_ext/1472/main.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/1472/main.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/1472/main.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/1472/main.o.temp ../main.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/1472 > ${OBJECTDIR}/_ext/1472/main.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/1472/main.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/1472/main.o.d
else
	cat ${OBJECTDIR}/_ext/1472/main.o.temp >> ${OBJECTDIR}/_ext/1472/main.o.d
endif
	${RM} __temp_cpp_output__
${OBJECTDIR}/_ext/1472/usb_descriptors.o: ../usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${MP_CC} $(MP_EXTRA_CC_PRE) -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/1472/usb_descriptors.o   ../usb_descriptors.c  > ${OBJECTDIR}/_ext/1472/usb_descriptors.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/1472/usb_descriptors.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/1472/usb_descriptors.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/1472/usb_descriptors.o.temp ../usb_descriptors.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/1472 > ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/1472/usb_descriptors.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d
else
	cat ${OBJECTDIR}/_ext/1472/usb_descriptors.o.temp >> ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d
endif
	${RM} __temp_cpp_output__
${OBJECTDIR}/_ext/926206843/usb_device.o: ../../../../Microchip/USB/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/926206843 
	${MP_CC} $(MP_EXTRA_CC_PRE) -D__DEBUG -D__MPLAB_DEBUGGER_REAL_ICE=1 -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/926206843/usb_device.o   ../../../../Microchip/USB/usb_device.c  > ${OBJECTDIR}/_ext/926206843/usb_device.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/926206843/usb_device.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/926206843/usb_device.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/926206843/usb_device.o.temp ../../../../Microchip/USB/usb_device.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/926206843 > ${OBJECTDIR}/_ext/926206843/usb_device.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/926206843/usb_device.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/926206843/usb_device.o.d
else
	cat ${OBJECTDIR}/_ext/926206843/usb_device.o.temp >> ${OBJECTDIR}/_ext/926206843/usb_device.o.d
endif
	${RM} __temp_cpp_output__
else
${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o: ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/1083301514 
	${MP_CC} $(MP_EXTRA_CC_PRE) -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o   ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c  > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.temp ../../../../Microchip/USB/CDC\ Device\ Driver/usb_function_cdc.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/1083301514 > ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d
else
	cat ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.temp >> ${OBJECTDIR}/_ext/1083301514/usb_function_cdc.o.d
endif
	${RM} __temp_cpp_output__
${OBJECTDIR}/_ext/1472/main.o: ../main.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/1472/main.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${MP_CC} $(MP_EXTRA_CC_PRE) -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/1472/main.o   ../main.c  > ${OBJECTDIR}/_ext/1472/main.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/1472/main.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/1472/main.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/1472/main.o.temp ../main.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/1472 > ${OBJECTDIR}/_ext/1472/main.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/1472/main.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/1472/main.o.d
else
	cat ${OBJECTDIR}/_ext/1472/main.o.temp >> ${OBJECTDIR}/_ext/1472/main.o.d
endif
	${RM} __temp_cpp_output__
${OBJECTDIR}/_ext/1472/usb_descriptors.o: ../usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/1472 
	${MP_CC} $(MP_EXTRA_CC_PRE) -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/1472/usb_descriptors.o   ../usb_descriptors.c  > ${OBJECTDIR}/_ext/1472/usb_descriptors.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/1472/usb_descriptors.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/1472/usb_descriptors.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/1472/usb_descriptors.o.temp ../usb_descriptors.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/1472 > ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/1472/usb_descriptors.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d
else
	cat ${OBJECTDIR}/_ext/1472/usb_descriptors.o.temp >> ${OBJECTDIR}/_ext/1472/usb_descriptors.o.d
endif
	${RM} __temp_cpp_output__
${OBJECTDIR}/_ext/926206843/usb_device.o: ../../../../Microchip/USB/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	${RM} ${OBJECTDIR}/_ext/926206843/usb_device.o.d 
	${MKDIR} ${OBJECTDIR}/_ext/926206843 
	${MP_CC} $(MP_EXTRA_CC_PRE) -p$(MP_PROCESSOR_OPTION) -I".." -I"../../../../Microchip/Include"  -I ${MP_CC_DIR}\\..\\h  -fo ${OBJECTDIR}/_ext/926206843/usb_device.o   ../../../../Microchip/USB/usb_device.c  > ${OBJECTDIR}/_ext/926206843/usb_device.err 2>&1 ; if [ $$? -eq 0 ] ; then cat ${OBJECTDIR}/_ext/926206843/usb_device.err | sed 's/\(^.*:.*:\)\(Warning\)\(.*$$\)/\1 \2:\3/g' ; else cat ${OBJECTDIR}/_ext/926206843/usb_device.err | sed 's/\(^.*:.*:\)\(Error\)\(.*$$\)/\1 \2:\3/g' ; exit 1 ; fi
	${MP_CPP}  -MMD ${OBJECTDIR}/_ext/926206843/usb_device.o.temp ../../../../Microchip/USB/usb_device.c __temp_cpp_output__ -D __18F14K50 -D __18CXX -I D:\\MAL\ May\ 05\\USB\\Device\ -\ CDC\ -\ Basic\ Demo\\Firmware -I D:\\MAL\ May\ 05\\Microchip\\Include -I C:\\Program\ Files\\Microchip\\mplabc18\\v3.38\\bin/../h  -D__18F14K50
	printf "%s/" ${OBJECTDIR}/_ext/926206843 > ${OBJECTDIR}/_ext/926206843/usb_device.o.d
ifneq (,$(findstring MINGW32,$(OS_CURRENT)))
	cat ${OBJECTDIR}/_ext/926206843/usb_device.o.temp | sed -e 's/\\\ /__SPACES__/g' -e's/\\$$/__EOL__/g' -e 's/\\/\//g' -e 's/__SPACES__/\\\ /g' -e 's/__EOL__/\\/g' >> ${OBJECTDIR}/_ext/926206843/usb_device.o.d
else
	cat ${OBJECTDIR}/_ext/926206843/usb_device.o.temp >> ${OBJECTDIR}/_ext/926206843/usb_device.o.d
endif
	${RM} __temp_cpp_output__
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.cof: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_LD} $(MP_EXTRA_LD_PRE) ../rm18f46j50_g.lkr  -p$(MP_PROCESSOR_OPTION_LD)  -w -x   -z__MPLAB_BUILD=1  -u_CRUNTIME -z__MPLAB_DEBUG=1 -z__MPLAB_DEBUGGER_REAL_ICE=1 $(MP_LINKER_DEBUG_OPTION) -l ${MP_CC_DIR}\\..\\lib  -odist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.cof ${OBJECTFILES}      
else
dist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.cof: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk
	${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_LD} $(MP_EXTRA_LD_PRE) ../rm18f46j50_g.lkr  -p$(MP_PROCESSOR_OPTION_LD)  -w    -z__MPLAB_BUILD=1  -u_CRUNTIME -l ${MP_CC_DIR}\\..\\lib  -odist/${CND_CONF}/${IMAGE_TYPE}/MPLAB.X.${IMAGE_TYPE}.cof ${OBJECTFILES}      
endif


# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf:
	${RM} -r build/PIC18F46J50_PIM
	${RM} -r dist/PIC18F46J50_PIM

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc

/************************************************    
CREATED BY  : Keerthana D
DATE CREATED: 25 Aug 2023
VERSION   :1.0.0.0   
PURPOSE   :Get TTY Get Subprocess
*************************************************/ 
CREATE PROCEDURE [dbo].[USP_TTY_WSDATA_GetSubPrcess]
AS            
BEGIN      
SET NOCOUNT ON;   
	
			SELECT TC.CustId,TC.ProjId,TC.FormId,CustName,ProjName,TC.ServiceUrl,TC.UserName,TC.PassWord,TC.TTYStatus,
			TC.ProcessId,TC.SubprocessId
			FROM TTY_WSDATA_Config TC WITH(NOLOCK)
			--JOIN FORMPROCESS FSM WITH(NOLOCK) ON TC.CustId = FSM.CustId AND TC.ProjId = FSM.ProjId AND TC.FormId =  FSM.FormId
			--AND TC.ProcessId = FSM.ProcessId AND TC.SubprocessId = FSM.SubprocessId
			JOIN CUSTOMERMASTER CM WITH(NOLOCK) ON TC.CustId  = CM.CustId
			JOIN PROJECTMASTER PM WITH(NOLOCK) ON TC.CustId  = PM.CustId AND TC.ProjId = PM.ProjId
			JOIN SUBPROCESS SS WITH(NOLOCK) ON TC.SubprocessId = SS.SubprocessId
			WHERE ISNULL(TC.Active,'') = 'Y' AND SS.SubprocessName IN ('TTYSORTRESCANWAIT')
SET NOCOUNT OFF;        
END    

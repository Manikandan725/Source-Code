/************************************************    
CREATED BY  : Keerthana D
DATE CREATED: 25 Aug 2023
VERSION   :1.0.0.0   
PURPOSE   :Insert TTY Send Xml Details
*************************************************/ 
create PROC USP_TTY_WSDATA_UpdateTTYStatus
@CustId INT,
@ProjId INT,
@SubProcessId INT,
@Dcn VARCHAR(100),
@ReturnXml VARCHAR(MAX)
AS            
BEGIN      
SET NOCOUNT ON;   
			
			DECLARE @ReturnXML1 XML
			DECLARE @Status Varchar(150),@ErrorDesc Varchar(Max)
			BEGIN TRY 
				
				UPDATE T SET ReturnXml = @ReturnXml
				FROM TTY_WSDATA_Dcn T WITH(ROWLOCK) WHERE CustId = @CustId AND ProjId = @ProjId AND Dcn = @Dcn
		
				select @ReturnXML1=REPLACE (REPLACE (CONVERT(VARCHAR(MAX), @ReturnXml), '&lt;', '<'), '&gt;', '>') 
				Select @Status=r.value('(./StatusXML)[1]','varchar(max)') FROM @ReturnXML1.nodes('//Result') AS A(r)
				--IF(@Status='FAILURE')
				--Begin
				--	Select @ErrorDesc = (Select  r.value('(./ResponseXML/RESPONSE/FAILURE_INFO/REASON)[1]','varchar(max)') As ErrorDesc
				--	FROM @ReturnXML1.nodes('//Result') AS A(r))
					
				--	UPDATE FM SET SUBPROCESSSTATUS = CASE WHEN @ErrorDesc = 'No documents found matching the request.  No documents are extracted.  '
				--	THEN 'READY' ELSE 'ERROR' END 
				--	FROM FILEMASTER FM WITH(ROWLOCK) WHERE CUSTID = @CustId AND PROJID = @ProjId AND DCN = @Dcn
				--	--AND  SubProcessId = @SubProcessId AND SUBPROCESSSTATUS = 'WIP' 				
				--	AND  SubProcessId = @SubProcessId AND SUBPROCESSSTATUS = 'READY' 				
				--End
				IF(@Status='SUCCESS')
				Begin
					INSERT INTO TTYSORTWSRESCANDETAILS
					(CUSTOMER ,DCN ,REJECTDATE ,REJECTTYPE ,REJRESREASON ,REJRESACTION ,RMODCN ,RMOPROCESSDATE ,DELIVERYTYPE ,DELIVERYTRACKINGNUMBER ,
					DELIVERYDATE ,[STATUS] ,UPDATDBY )
					SELECT c.value('(./FIELD/VALUE)[1]', 'nvarchar(max)') AS TTY_CUSTOM,
					c.value('(./FIELD/VALUE)[5]', 'nvarchar(max)') AS TTY_DCN,
					c.value('(./FIELD/VALUE)[12]', 'nvarchar(max)') AS TTY_REJDTE,
					c.value('(./FIELD/VALUE)[13]', 'nvarchar(max)') AS TTY_REJTYP,
					c.value('(./FIELD/VALUE)[15]', 'nvarchar(max)') AS TTY_REJREA,
					c.value('(./FIELD/VALUE)[14]', 'nvarchar(max)') AS TTY_REJSOU,
					c.value('(./FIELD/VALUE)[17]', 'nvarchar(max)') AS TTY_RMODCN,
					c.value('(./FIELD/VALUE)[18]', 'nvarchar(max)') AS TTY_RMOPDT,
					c.value('(./FIELD/VALUE)[19]', 'nvarchar(max)') AS TTY_DELTYP,
					c.value('(./FIELD/VALUE)[20]', 'nvarchar(max)') AS TTY_DETRNO,
					c.value('(./FIELD/VALUE)[21]', 'nvarchar(max)') AS TTY_DELDTE,
					c.value('(./FIELD/VALUE)[22]', 'nvarchar(max)') AS TTY_STATUS,
					c.value('(./FIELD/VALUE)[23]', 'nvarchar(max)') AS TTY_UPDTBY
					FROM @ReturnXML1.nodes('//Result') AS A(r)
					CROSS APPLY A.r.nodes('/*:Envelope/*:Body/*:DocDNAExtractResponse/*:Result/*:DocumentList/*:MASSEXTRACT/*:DOCUMENT') AS t(c);
					UPDATE FM SET SUBPROCESSSTATUS = 'COMPLETED'
					FROM FILEMASTER FM WITH(ROWLOCK) WHERE CUSTID = @CustId AND PROJID = @ProjId AND DCN = @Dcn
					--AND  SubProcessId = @SubProcessId AND SUBPROCESSSTATUS = 'WIP'
					AND  SubProcessId = @SubProcessId AND SUBPROCESSSTATUS = 'READY'
				End
					--UPDATE FM SET SUBPROCESSSTATUS = 'ERROR'
				 --   FROM FILEMASTER FM WITH(ROWLOCK) WHERE CUSTID = @CustId AND PROJID = @ProjId AND DCN = @Dcn
					--AND  SubProcessId = @SubProcessId AND SUBPROCESSSTATUS = 'WIP' 
				INSERT INTO ERRORDETAILS(FORMNAME,FUNCTIONNAME,ERRORDESCRIPTION,DATETIME,ERRORTYPE,CUSTID,PROJID)                                                   
				select 'USP_TTY_WSDATA_UpdateStatus',Error_Procedure(),'Dcn: '+@Dcn+', '+@ErrorDesc,GETDATE(),'STOREDPROCEDURE',@CustId,@ProjId
				FROM FILEMASTER FM WITH(ROWLOCK) WHERE FM.CUSTID = @CustId AND FM.PROJID = @ProjId AND FM.DCN = @Dcn
				AND  FM.SubProcessId = @SubProcessId AND FM.SUBPROCESSSTATUS ='ERROR'
			END TRY        
			BEGIN CATCH        
					INSERT INTO ERRORDETAILS(FORMNAME,FUNCTIONNAME,ERRORDESCRIPTION,DATETIME,ERRORTYPE,CUSTID,PROJID) VALUES                                                  
					('USP_TTY_WSDATA_UpdateStatus',Error_Procedure(),Error_Message(),GETDATE(),'STOREDPROCEDURE',@CustId,@ProjId)       
			END CATCH
SET NOCOUNT OFF;        
END    

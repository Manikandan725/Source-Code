-- =============================================
-- Author:		<Manikanadan A>
-- Create date: <17-Aug-2023>
-- Description:	<Get Web Service Data>
-- =============================================
create PROCEDURE USP_TTY_WSDATA_GetXmlDetails
@CustId INT,
@ProjId INT,
@SubProcessId INT,
@Dcn VARCHAR(100),
@UserName VARCHAR(100),
@PassWord VARCHAR(100),
@TTYStatus VARCHAR(100)
AS            
BEGIN      
SET NOCOUNT ON;   
			/*****************Variable Declaration ********************/
				DECLARE @SendXml VARCHAR(MAX),@CustName Varchar(100)
				SELECT @CustName = CustName FROM CUSTOMERMASTER WITH(NOLOCK) WHERE CUSTID = @CustId
			BEGIN TRY 
				Select @SendXml ='<?xml version="1.0" encoding="utf-8"?>
				<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:doc="http://hq-srames-lt.lason.com/DocDNAWS/">
				   <soapenv:Header/>
				   <soapenv:Body>
					  <doc:DocDNAExtract>
						 <Security>
							<user>'+ @UserName +'</user>
							<pass>'+ @PassWord + '</pass>
							<end_user>?</end_user>
						 </Security>
						 <QueryXML>
							<RptID>TTYS01</RptID>
							<QueryXML>
									  <![CDATA[
				<BATCHEXTRACT IDXFORMAT="XML" INDEXONLY="YES" >
				<MATCH>
				<FIELD>
				<NAME>TTY_CUSTOM</NAME>
				<VALUE>' + @CustName + '</VALUE>
				</FIELD>
				<FIELD>
				<NAME>TTY_DCN</NAME>
				<VALUE>' + @Dcn + '</VALUE>
				</FIELD>
				<FIELD>
				<NAME>TTY_STATUS</NAME>
				<VALUE>' + @TTYStatus + '</VALUE>
				</FIELD>
				</MATCH>
				</BATCHEXTRACT>
				]]>
							</QueryXML>
						 </QueryXML>
					  </doc:DocDNAExtract>
				   </soapenv:Body>
				</soapenv:Envelope>' 
				IF EXISTS(SELECT TOP 1 1 FROM TTY_WSDATA_Dcn WITH(NOLOCK) WHERE CustId = @CustId AND ProjId = @ProjId AND Dcn = @Dcn)
				BEGIN
					DELETE FROM TTY_WSDATA_Dcn WHERE CustId = @CustId AND ProjId = @ProjId AND Dcn = @Dcn
				END
				--UPDATE FM SET SUBPROCESSSTATUS = 'WIP'
				--FROM FILEMASTER FM WITH(ROWLOCK) WHERE CUSTID = @CustId AND PROJID = @ProjId AND DCN = @Dcn
				--AND  SubProcessId = @SubProcessId AND SUBPROCESSSTATUS = 'READY'
				INSERT INTO TTY_WSDATA_Dcn(CustId ,ProjId ,Dcn,SendXml)
				SELECT @CustId,@ProjId,@Dcn,@SendXml
				SELECT 'text/xml'as [Content-Type],@SendXml AS output
		END TRY        
			BEGIN CATCH        
					INSERT INTO ERRORDETAILS(FORMNAME,FUNCTIONNAME,ERRORDESCRIPTION,DATETIME,ERRORTYPE,CUSTID,PROJID) VALUES                                                  
					('USP_TTY_WSDATA_GetXmlDetails',Error_Procedure(),Error_Message(),GETDATE(),'STOREDPROCEDURE',@CustId,@ProjId)       
			END CATCH
SET NOCOUNT OFF;        
END 

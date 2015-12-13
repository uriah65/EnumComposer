
1). If OLEDB test fail, you me be missing 'Microsoft.ACE.OLEDB.12.0' provider. Follow 
<https://social.msdn.microsoft.com/Forums/en-US/1d5c04c7-157f-4955-a14b-41d912d50a64/how-to-fix-error-the-microsoftaceoledb120-provider-is-not-registered-on-the-local-machine?forum=vstsdb>
which recommends to install 'Microsoft Access Database Engine 2010 Redistributable' <https://www.microsoft.com/en-us/download/confirmation.aspx?id=23734>
Worked for me on clean Windows 10 64bit PC.

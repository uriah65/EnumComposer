
*********************************
####PROJECT: EnumComposerVSP  

To debug

- set 
	"Properties\Debug\Start Action\Start external program:" to
	C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe
- set 
	"Properties\Debug\Start Options\Command line arguments:"  to
	/rootSuffix Exp "XXX.sln"

where XXX.sln is the full path to the test solution used in debugging.
See <http://stackoverflow.com/questions/17625752/cannot-run-vspackage-when-developing-on-multiple-machines>

*********************************
####PROJECT: TestComposer

- If OLEDB test (Reading_Access_OleDb) fail, you me be missing 'Microsoft.ACE.OLEDB.12.0' provider. Follow 
<https://social.msdn.microsoft.com/Forums/en-US/1d5c04c7-157f-4955-a14b-41d912d50a64/how-to-fix-error-the-microsoftaceoledb120-provider-is-not-registered-on-the-local-machine?forum=vstsdb>
which recommends to install 'Microsoft Access Database Engine 2010 Redistributable' <https://www.microsoft.com/en-us/download/confirmation.aspx?id=23734>.
Worked OK on clean Windows 10 64bit PC.


*********************************
####PROJECT: TestConsole

- Please build EnumComposerConsole project before running these tests

# Release Notes Generator

Team Foundation Server Release Notes Generator provides a GUI/Command line support to generate Release Notes from the TFS work items identified by a TFS Work Item Query.  Currently the generator supports writing the output in PDF format.  For detailed information on the design goals and how the solution fits together, read through this [blogpost](http://fromthedevtrenches.blogspot.com/2013/04/automatically-generate-tfs-release.html).


## User Interface Support

![UI](https://github.com/cjlotz/ReleaseNotes/blob/master/Images/ReleaseNotesGenerator.png)

## Command Line Support

Example using a MSBuild target:
 
```
<Target Name="PackageReleaseNotes">

	<Message Text="Packaging Release Notes..." Importance="high" />
	
	<PropertyGroup>  
	  <DOUBLE_QUOTES>%22</DOUBLE_QUOTES>
	
	  <ProductName>YourProduct</ProductName>
	  <ReleaseNotesGeneratorPath>$(ToolsFolder)\ReleaseNotes</ReleaseNotesGeneratorPath>
	  <ReleaseNotesGeneratorCmd>$(ReleaseNotesGeneratorPath)\ReleaseNotes.TfsTool.exe</ReleaseNotesGeneratorCmd>
	  <NewReleaseNotesFile>$(DocsFolder)\ReleaseNotes_Output.pdf</NewReleaseNotesFile>
	  <LinkWorkItems>false</LinkWorkItems>
	
	  <TFSServer>http://your.tfsserver.com/</TFSServer>
	  <TFSProject Condition=" '$(TFSProject)' == '' ">YourTfsProject</TFSProject>
	  <TFSQueryHierarchy Condition=" '$(TFSQueryHierarchy)' == '' ">Your Queries</TFSQueryHierarchy>
	  <TFSQueryName Condition=" '$(TFSQueryName)' == '' ">Release Notes</TFSQueryName>
	</PropertyGroup>
	
	<!-- Generate the Release Notes file -->
	<Exec Command="$(ReleaseNotesGeneratorCmd) /Action=Export 
		/ExportFile=$(DOUBLE_QUOTES)$(ReleaseNotesFileWithExtension)$(DOUBLE_QUOTES) 
		/WarningsFile=$(DOUBLE_QUOTES)$(ReleaseNotesWarningsFile)$(DOUBLE_QUOTES) 
		/LinkWorkItems=$(LinkWorkItems)
		/ProductName=$(ProductName) 
		/BuildNumber=$(BuildVersionNumber) 
		/NewFile=$(DOUBLE_QUOTES)$(NewReleaseNotesFile)$(DOUBLE_QUOTES) 
		/TfsServerUrl=$(TFSServer) 
		/TfsProject=$(DOUBLE_QUOTES)$(TFSProject)$(DOUBLE_QUOTES) 
		/TfsQueryHierarchy=$(DOUBLE_QUOTES)$(TFSQueryHierarchy)$(DOUBLE_QUOTES) 
		/TfsQueryName=$(DOUBLE_QUOTES)$(TFSQueryName)$(DOUBLE_QUOTES)" 
		WorkingDirectory="$(ReleaseNotesGeneratorPath)"/>

</Target>
```

## Example Output

![PDF](https://github.com/cjlotz/ReleaseNotes/blob/master/Images/ReleaseNotesGeneratorOutput.png)

  

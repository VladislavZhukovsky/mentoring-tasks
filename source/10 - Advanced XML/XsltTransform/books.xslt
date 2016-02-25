<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:b="http://library.by/catalog"
>
    <xsl:output method="html" indent="yes"/>

    <msxsl:script language="CSharp" implements-prefix="b">
       <![CDATA[
       public string DateTimeNow()
       {
          return System.DateTime.UtcNow.ToString();
       }
       ]]>
    </msxsl:script>
    
    <xsl:template match="/b:catalog">
      <html>
        <body>
          <h1>Books report by genres <xsl:value-of select="b:DateTimeNow()"/></h1>
          <xsl:call-template name="table">
            <xsl:with-param name="genreName">Computer</xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="table">
            <xsl:with-param name="genreName">Fantasy</xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="table">
            <xsl:with-param name="genreName">Romance</xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="table">
            <xsl:with-param name="genreName">Horror</xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="table">
            <xsl:with-param name="genreName">Science Fiction</xsl:with-param>
          </xsl:call-template>
          <xsl:call-template name="tableSummaryCatalog"></xsl:call-template>
        </body>
      </html>
    </xsl:template>


  <xsl:template name="table">
    <xsl:param name="genreName"/>
    
    <table>

      <xsl:call-template name="tableHeader">
        <xsl:with-param name="genreName">
          <xsl:value-of select="$genreName"/>
        </xsl:with-param>
      </xsl:call-template>

      <xsl:call-template name="tableBody">
        <xsl:with-param name="genreName">
          <xsl:value-of select="$genreName"/>
        </xsl:with-param>
      </xsl:call-template>

      <xsl:call-template name="tableSummary">
        <xsl:with-param name="genreName">
          <xsl:value-of select="$genreName"/>
        </xsl:with-param>
      </xsl:call-template>
    </table>
  </xsl:template>

  <xsl:template name="tableHeader">
    <xsl:param name="genreName"/>
    
    <xsl:call-template name="tableHeader1">
      <xsl:with-param name="genreName">
        <xsl:value-of select="$genreName"/>
      </xsl:with-param>
    </xsl:call-template>
    <xsl:call-template name="tableHeader2"/>
    
  </xsl:template>

  <xsl:template name="tableHeader1">
    <xsl:param name="genreName"/>
    <tr>
      <td>
        <xsl:value-of select="$genreName"></xsl:value-of>
      </td>
    </tr>
  </xsl:template>

  <xsl:template name="tableHeader2">
    <tr>
      <td align="center">Author</td>
      <td align="center">Title</td>
      <td align="center">Publish date</td>
      <td align="center">Registration date</td>
    </tr>
  </xsl:template>

  <xsl:template name="tableBody">
    <xsl:param name="genreName"/>
    <xsl:for-each select="b:book[b:genre=$genreName]">
      <tr>
        <td>
          <xsl:value-of select="b:author"/>
        </td>
        <td>
          <xsl:value-of select="b:title"/>
        </td>
        <td>
          <xsl:value-of select="b:publish_date"/>
        </td>
        <td>
          <xsl:value-of select="b:registration_date"/>
        </td>
      </tr>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="tableSummary">
    <xsl:param name="genreName"/>
    <tr>
      <td>Count: <xsl:value-of select="count(//b:book[b:genre=$genreName])"/></td>
    </tr>
  </xsl:template>

  <xsl:template name="tableSummaryCatalog">
    <table>
      <tr>
        <td>All count: <xsl:value-of select="count(//b:book)"/>
      </td>
      </tr>
    </table>
  </xsl:template>
  
</xsl:stylesheet>

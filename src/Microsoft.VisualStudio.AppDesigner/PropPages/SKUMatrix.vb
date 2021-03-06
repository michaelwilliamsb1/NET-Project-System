' Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

Imports Microsoft.VisualStudio.Shell

Imports VSLangProj80

Namespace Microsoft.VisualStudio.Editors.PropertyPages

    'Class containing information for specific VS product SKUs
    'Currently only supports whether a property is visible on the
    'property pages.  Any other SKU specific info should go here.
    Public NotInheritable Class SKUMatrix

        ' This guid is duplicated in "src\appid\VW8Express\stub\guids.h" and "src\wizard\vbdesigner\Designer\PropPages\SKUMatrix.vb"
        Private Shared ReadOnly s_guidShowEnableUnmanagedDebugging As New Guid("2172A533-76E4-483F-BFB9-71D9B8253B13")

        Private Sub New()
            'Disallow creation
        End Sub

        Public Shared Function IsHidden(PropertyId As Integer) As Boolean

            If VSProductSKU.IsExpress Then
                'These properties are to be hidden for all Express SKU
                'VSWhidbey # 239181 - Disable unmanaged debugging in all express SKUs
                '(except VC ... but VC is handled by a different property page so
                'we do not need to worry about it here).

                Select Case PropertyId
                    Case VsProjPropId.VBPROJPROPID_RemoteDebugEnabled,
                        VsProjPropId.VBPROJPROPID_EnableSQLServerDebugging,
                        VsProjPropId.VBPROJPROPID_StartAction

                        Return True

                    Case VsProjPropId.VBPROJPROPID_EnableUnmanagedDebugging
                        Return Not UIContext.FromUIContextGuid(s_guidShowEnableUnmanagedDebugging).IsActive
                End Select

                'These properties are to be hidden for the VB Express SKU
                If VSProductSKU.IsVB Then
                    Select Case PropertyId
                        Case VsProjPropId.VBPROJPROPID_RegisterForComInterop,
                         VsProjPropId.VBPROJPROPID_IncrementalBuild,
                         VsProjPropId.VBPROJPROPID_DocumentationFile,
                         VsProjPropId2.VBPROJPROPID_PreBuildEvent,
                         VsProjPropId2.VBPROJPROPID_PostBuildEvent,
                         VsProjPropId2.VBPROJPROPID_RunPostBuildEvent
                            Return True
                    End Select
                End If
            ElseIf VSProductSKU.IsStandard Then
                Select Case PropertyId
                    Case VsProjPropId.VBPROJPROPID_EnableSQLServerDebugging,
                        VsProjPropId.VBPROJPROPID_RemoteDebugEnabled

                        Return True
                End Select
            End If

            Return False
        End Function

    End Class

End Namespace

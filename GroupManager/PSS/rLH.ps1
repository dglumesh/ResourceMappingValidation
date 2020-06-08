$Subscriptions = Get-AzSubscription
foreach ($sub in $Subscriptions){
set-Azcontext -SubscriptionId $sub.Id
New-AzDeployment -Name 'rhipeMSP' -Location 'Australia East' -TemplateFile 'dRM.json' -TemplateParameterFile 'dRMpara.json' -Verbose
}
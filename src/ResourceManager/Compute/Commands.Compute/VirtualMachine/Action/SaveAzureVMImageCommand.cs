﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using AutoMapper;
using Microsoft.Azure.Commands.Compute.Common;
using Microsoft.Azure.Commands.Compute.Models;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using System.Management.Automation;
using System.IO;

namespace Microsoft.Azure.Commands.Compute
{
    [Cmdlet(VerbsData.Save, ProfileNouns.VirtualMachineImage)]
    [OutputType(typeof(PSComputeLongRunningOperation))]
    public class SaveAzureVMImageCommand : VirtualMachineBaseCmdlet
    {
        public string Name { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 0,
           ValueFromPipelineByPropertyName = true,
           HelpMessage = "The resource group name.")]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 1,
           ValueFromPipelineByPropertyName = true,
           HelpMessage = "The virtual machine name.")]
        [ValidateNotNullOrEmpty]
        public string VMName { get; set; }

        [Parameter(
           Mandatory = true,
           Position = 2,
           ValueFromPipelineByPropertyName = true,
           HelpMessage = "The Destination Container Name.")]
        [ValidateNotNullOrEmpty]
        public string DestinationContainerName { get; set; }

        [Alias("VirtualHardDiskNamePrefix")]
        [Parameter(
           Mandatory = true,
           Position = 3,
           ValueFromPipelineByPropertyName = true,
           HelpMessage = "The Virtual Hard Disk Name Prefix.")]
        [ValidateNotNullOrEmpty]
        public string VHDNamePrefix { get; set; }

        [Parameter(
           Position = 4,
           ValueFromPipelineByPropertyName = true,
           HelpMessage = "To Overwrite.")]
        [ValidateNotNullOrEmpty]
        public SwitchParameter Overwrite { get; set; }

        [Parameter(
           Position = 5,
           ValueFromPipelineByPropertyName = true,
           HelpMessage = "The file path in which the template of the captured image is stored")]
        [ValidateNotNullOrEmpty]
        public string Path { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            ExecuteClientAction(() =>
            {
                var parameters = new VirtualMachineCaptureParameters
                {
                    DestinationContainerName = DestinationContainerName,
                    Overwrite = Overwrite.IsPresent,
                    VirtualHardDiskNamePrefix = VHDNamePrefix
                };

                var op = this.VirtualMachineClient.Capture(
                    this.ResourceGroupName,
                    this.VMName,
                    parameters);

                var result = Mapper.Map<PSComputeLongRunningOperation>(op);

                if (!string.IsNullOrWhiteSpace(this.Path))
                {
                    File.WriteAllText(this.Path, result.Output);
                }
                WriteObject(result);
            });
        }
    }
}

import {inject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ConfirmDialog} from '../confirmDialog';
import {CasesService} from '../../../../services/cases-service';


@inject(DialogService, CasesService)
export class TechnicalIssue {
  cases: {}[];
  selectedCase: {};

  constructor(private dialogService: DialogService, private casesService: CasesService) {
    casesService.getTechnicianCases().then(res => {
      let tmpCases = JSON.parse(JSON.stringify(res));
      this.cases = tmpCases;
    });
  }

  selectCase(selectedCase) {
    this.selectedCase = selectedCase;
    console.log('this.selectedCase ', this.selectedCase);
  }

  confirmDialog(){
    this.dialogService.open({ viewModel: ConfirmDialog, model: ''});
  }

}

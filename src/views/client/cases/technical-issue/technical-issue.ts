import {inject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {Router, RouterConfiguration} from 'aurelia-router';
import {ConfirmDialog} from '../confirmDialog';
import {CasesService} from '../../../../services/cases-service';


@inject(DialogService, CasesService, Router)
export class TechnicalIssue {
  cases: {}[];
  selectedCase: {};

  constructor(private dialogService: DialogService, private casesService: CasesService, private router: Router) {
    casesService.getTechnicianCases().then(res => {
      let tmpCases = JSON.parse(JSON.stringify(res));
      this.cases = tmpCases;
    });
  }

  selectCase(selectedCase) {
    this.selectedCase = selectedCase;
    console.log('this.selectedCase ', this.selectedCase);
  }

  confirmDialog() {
    if (this.selectedCase) {
      this.dialogService.open({viewModel: ConfirmDialog, model: ''})
        .whenClosed(response => {
          if (!response.wasCancelled) {
            console.log('good');
            //POST the task here
            //       .then(redirect) --> this.router.navigateToRoute('base');
            this.router.navigateToRoute('base');
          }
        });
    }

  }

  returnToMainPanel() {
    this.router.navigateToRoute('base');
  }
}

import {inject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ConfirmDialog} from '../confirmDialog';
import {CasesService} from '../../../../services/cases-service';


@inject(DialogService, CasesService)
export class TechnicalIssue {
  cases: {}[];
  selectedId: number;

  constructor(private dialogService = DialogService, private casesService: CasesService) {
    casesService.getTechnicianCases().then(res => {
      this.cases = JSON.parse(JSON.stringify(res));
    });

    this.cases = [
      {
        id: 1,
        text: 'Wymiana żarówki'
      },
      {
        id: 2,
        text: 'Naprawa łóżka'
      },
      {
        id: 3,
        text: 'Naprawa telewizora'
      },
      {
        id: 4,
        text: 'Inne'
      }
    ];
  }

  selectCase(selectedCase) {
    this.selectedId = selectedCase.CaseID;
    console.log(selectedCase);
    // return true;
  }

  confirmDialog(){
    this.dialogService.open({ viewModel: ConfirmDialog, model: ''});
  }
}

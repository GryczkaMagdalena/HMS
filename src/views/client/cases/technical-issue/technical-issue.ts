import {inject} from 'aurelia-framework';
import {DialogService} from 'aurelia-dialog';
import {ConfirmDialog} from '../confirmDialog';

@inject(DialogService)
export class TechnicalIssue {
  cases: {}[];
  selectedId: number;

  constructor(private dialogService = DialogService) {
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
    ]
  }

  selectCase(selectedCase) {
    this.selectedId = selectedCase.id;
    console.log(selectedCase);
    // return true;
  }

  confirmDialog(){
    this.dialogService.open({ viewModel: ConfirmDialog, model: ''});
  }
}

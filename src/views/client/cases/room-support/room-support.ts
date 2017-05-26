import {inject} from 'aurelia-framework';
import {Router, RouterConfiguration} from 'aurelia-router';
import {DialogService} from 'aurelia-dialog';
import {ConfirmDialogComponent} from '../../../../components/confirm-dialog/confirm-dialog-component';
import {CasesService} from '../../../../services/cases-service';
import {TasksService} from '../../../../services/tasks-service';


@inject(DialogService, CasesService, TasksService, Router)
export class RoomSupport {
  cases: {}[];
  selectedCase: {};

  constructor(private dialogService: DialogService, private casesService: CasesService, private tasksService: TasksService, private router: Router) {
    casesService.getCleanerCases().then(res => {
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
      this.dialogService.open({viewModel: ConfirmDialogComponent, model: ''})
        .whenClosed(response => {
          if (!response.wasCancelled) {
            console.log('good');
            this.tasksService.createTask(this.selectedCase)
              .then(response => {
                console.log('response tuuu', response);
                this.router.navigateToRoute('base');
              })
              .catch(err => {
                alert(err.status);
              });
          }
        });
    }
  }

  returnToMainPanel(){
    this.router.navigateToRoute('base');
  }
}

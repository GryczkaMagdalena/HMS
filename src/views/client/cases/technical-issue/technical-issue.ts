import {inject} from 'aurelia-framework';
import {Router, RouterConfiguration} from 'aurelia-router';
import {DialogService} from 'aurelia-dialog';
import {ConfirmDialogComponent} from '../../../../components/confirm-dialog/confirm-dialog-component';
import {CasesService} from '../../../../services/cases-service';
import {TasksService} from '../../../../services/tasks-service';


@inject(DialogService, CasesService, TasksService, Router)
export class TechnicalIssue {
  cases: {}[];
  selectedCase: {};

  constructor(private dialogService: DialogService, private casesService: CasesService, private tasksService: TasksService, private router: Router) {
    casesService.getTechnicianCases().then(res => {
      let tmpCases = JSON.parse(JSON.stringify(res));
      this.cases = tmpCases;
    });
  }

  selectCase(selectedCase) {
    this.selectedCase = selectedCase;
  }

  confirmDialog() {
    if (this.selectedCase) {
      this.dialogService.open({viewModel: ConfirmDialogComponent, model: ''})
        .whenClosed(response => {
          if (!response.wasCancelled) {
            this.tasksService.createTask(this.selectedCase)
              .then(response => {
                this.router.navigateToRoute('base');
              })
              .catch(err => {
                alert(err.status);
              });
          }
        });
    }
  }
}

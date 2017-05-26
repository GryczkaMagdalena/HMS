import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {DialogService} from 'aurelia-dialog';
import {EventAggregator} from 'aurelia-event-aggregator';
import {LogoutDialogComponent} from "../logout-dialog/logout-dialog-component";

@inject(DialogService, Router, EventAggregator)
export class LogoutComponent {

  constructor(private dialogService: DialogService, private router: Router, private eventAggregator: EventAggregator) {
  }

  logout() {
    this.dialogService.open({viewModel: LogoutDialogComponent, model: ''})
      .whenClosed(response => {
      if (!response.wasCancelled) {
        console.log('Logged out succesfully');
        this.eventAggregator.publish('login::loggedIn', { loggedIn: false });
        this.router.navigateToRoute('login');
      }
    });
  }

}

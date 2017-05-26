import {inject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';

@inject(DialogController)
export class LogoutDialogComponent {
	
	constructor(private dialogController: DialogController) {
	}

	yes(){
    sessionStorage.clear();
		this.dialogController.ok();
	}

	no(){
		this.dialogController.cancel();
	}
}

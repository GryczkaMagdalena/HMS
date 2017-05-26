import {inject} from 'aurelia-framework';
import {DialogController} from 'aurelia-dialog';
import {Router} from 'aurelia-router';

@inject(DialogController, Router)
export class ConfirmDialogComponent {
	
	constructor(private dialogController: DialogController, private router: Router) {
	}

	yes(){
		this.dialogController.ok();
	}

	no(){
		this.dialogController.cancel();
	}
}

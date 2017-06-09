import {Dragula} from 'aurelia-dragula';
import {inject} from 'aurelia-framework';
import {EmployeeTasksService} from '../../../services/employee-tasks-service';

@inject(EmployeeTasksService)
export class MainPanel {
	tasks;
	constructor(private employeeTasksService: EmployeeTasksService, private dragula: Dragula) {
		employeeTasksService.getTasks()
			.then(data => this.tasks = data);
	}

}

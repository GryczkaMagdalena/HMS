import {CasesService} from '../../../../services/cases-service';
import {inject} from 'aurelia-framework';


@inject(CasesService)
export class CreatedTasks {
  createdTasks;

  constructor(private casesService: CasesService) {
    casesService.getCustomersTasks().then(res => {
      this.createdTasks = res;
    });
  }
}

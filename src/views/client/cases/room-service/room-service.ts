import {inject} from 'aurelia-framework';
import {CasesService} from '../../../../services/cases-service';


@inject(CasesService)
export class RoomService {

    constructor(private casesService: CasesService) {
    }

}

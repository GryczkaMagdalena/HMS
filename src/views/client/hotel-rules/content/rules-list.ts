import {inject} from 'aurelia-framework';
import {RouterConfiguration, Router} from 'aurelia-router';
import {HotelRulesService} from '../../../../services/hotel-rules-service';

@inject(HotelRulesService, Router)
export class RulesList {
	rules;
	rule;
	constructor(private hotelRulesService: HotelRulesService, private router: Router) {
		hotelRulesService.getRules().then(data => this.rules = data);
	}

	setRule(ruleID){
		this.rule = this.rules.find(item => item.ruleID == ruleID).ruleID;
		return true;
	}
}

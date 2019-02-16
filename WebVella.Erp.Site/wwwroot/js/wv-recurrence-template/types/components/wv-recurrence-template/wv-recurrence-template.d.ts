import '../../stencil.core';
import RecurrenceTemplate from '../../models/RecurrenceTemplate';
export declare class WvDatasourceManage {
    recurrenceTemplate: string;
    templateDefault: string;
    typeOptions: string;
    endTypeOptions: string;
    periodTypeOptions: string;
    changeTypeOptions: string;
    recurrence: RecurrenceTemplate;
    typeSelectOptions: Array<Object>;
    endTypeSelectOptions: Array<Object>;
    periodTypeSelectOptions: Array<Object>;
    changeTypeSelectOptions: Array<Object>;
    hiddenValue: string;
    initialType: number;
    componentWillLoad(): void;
    valueChangeHandler(ev: any, fieldName: any, dataType: any): void;
    render(): JSX.Element;
}

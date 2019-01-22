import '../../stencil.core';
import { EventEmitter } from '../../stencil.core';
export declare class WvSitemapManagerArea {
    area: Object;
    wvSitemapManagerAreaManageEvent: EventEmitter;
    wvSitemapManagerAreaDeleteEvent: EventEmitter;
    wvSitemapManagerNodeManageEvent: EventEmitter;
    wvSitemapManagerNodeDeleteEvent: EventEmitter;
    manageArea(): void;
    deleteArea(event: any): void;
    createNode(): void;
    manageNode(node: any): void;
    deleteNode(event: any, node: any): void;
    render(): JSX.Element;
}

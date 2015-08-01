var proview = {};
proview.app = angular.module("proviewApp", ['ajaxLoader', 'ui.bootstrap', 'trNgGrid', 'angular-confirm'])
.run(function () {
    //Change default settings
    TrNgGrid.defaultPagerMinifiedPageCountThreshold = 5;
})
.run(["$templateCache", function ($templateCache) {
    //Overwrite pager template
    $templateCache.put(
         TrNgGrid.footerPagerTemplateId,
         '<span class="pull-right form-group">'
                + ' <ul class="pagination">'
                + '   <li ng-class="{disabled:!pageCanGoBack}" ng-if="extendedControlsActive">'
                + '     <a href="" ng-click="pageCanGoBack&&navigateToPage(0)" ng-attr-title="{{\'First Page\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}">'
                //+ '         <span class="glyphicon glyphicon-fast-backward"></span>' 
                + '         <span>&laquo;</span>'
                + '     </a>'
                + '   </li>'
                + '   <li ng-class="{disabled:!pageCanGoBack}" ng-if="extendedControlsActive">'
                + '     <a href="" ng-click="pageCanGoBack&&navigateToPage(gridOptions.currentPage - 1)" ng-attr-title="{{\'Previous Page\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}">'
                //+ '         <span class="glyphicon glyphicon-step-backward"></span>' 
                + '         <span>&lsaquo;</span>'
                + '     </a>'
                + '   </li>'
                + '   <li ng-if="pageSelectionActive" ng-repeat="pageIndex in pageIndexes track by $index" ng-class="{disabled:pageIndex===null, active:pageIndex===gridOptions.currentPage}">'
                + '      <span ng-if="pageIndex===null">...</span>'
                + '      <a href="" ng-click="navigateToPage(pageIndex)" ng-if="pageIndex!==null" ng-attr-title="{{\'Page\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}">{{pageIndex+1}}</a>'
                + '   </li>'
                + '   <li ng-class="{disabled:!pageCanGoForward}" ng-if="extendedControlsActive">'
                + '     <a href="" ng-click="pageCanGoForward&&navigateToPage(gridOptions.currentPage + 1)" ng-attr-title="{{\'Next Page\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}">'
                //+ '         <span class="glyphicon glyphicon-step-forward"></span>' 
                + '         <span>&rsaquo;</span>'
                + '     </a>'
                + '   </li>'
                + '   <li ng-class="{disabled:!pageCanGoForward}" ng-if="extendedControlsActive">'
                + '     <a href="" ng-click="pageCanGoForward&&navigateToPage(lastPageIndex)" ng-attr-title="{{\'Last Page\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}">'
                //+ '         <span class="glyphicon glyphicon-fast-forward"></span>' 
                + '         <span>&raquo;</span>'
                + '     </a>'
                + '   </li>'
                + '   <li class="disabled" style="white-space: nowrap;">'
                + '     <span ng-hide="totalItemsCount">{{\'No items to display\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}</span>'
                + '     <span ng-show="totalItemsCount">'
                + '       {{startItemIndex+1}} - {{endItemIndex+1}} {{\'displayed\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}'
                + '       <span>, {{totalItemsCount}} {{\'in total\'|' + TrNgGrid.translateFilter + ':gridOptions.locale}}</span>'
                + '     </span > '
                + '   </li>'
                + ' </ul>'
                + '</span>'
    )
}])
;
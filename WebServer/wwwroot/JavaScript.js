'use strict'
let username = '';
let path = '';
let currentState = false;

function myFunc(msj) {
    alert(msj)
}
function DisplayAlert(msj) {
    alert(msj);
}
window.WriteCookie = {

    WriteCookie: function (name, value, days) {

        var expires;
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }
}
window.ReadCookie = {
    ReadCookie: function (cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
}

function OnHamburgerClick() {
    let parent = document.getElementById("navbar-items");
    let containerDropMenu = document.querySelector(".navbar-drop-down-container");
    let dpc = containerDropMenu.classList;
    dpc.remove("navbar-hide");
    let mainNavbar = document.querySelector(".navbar");
    let mdc = mainNavbar.classList;
    mdc.add("navbar-hide");
    let dropDown = document.querySelector(".navbar-drop-down-menu");
    let childs = parent.querySelectorAll("div");
    childs.forEach(function (div) {
        dropDown.appendChild(div);
    });
}
function HideDropDownMenu() {
    let parent = document.getElementById("navbar-items");
    let dropDown = document.querySelector(".navbar-drop-down-menu");
    let childs = parent.querySelectorAll("div");
    childs.forEach(function (div) {
        dropDown.appendChild(div);
    });
}

function SetUserData(path, name) {
    username = name;
    this.path = path;
    let avatar = document.getElementById("navbar-user-avatar-img");
    avatar.src = path;
    document.getElementById("navbar-user-name-text").textContent = name;
}
function AnimateSecondarySidebar(hide) {
    let sidebar = document.querySelector(".sidebar-secondary-container");
    let val = hide ? 150 : 0;
    if (hide) {
        //sidebar.setAttribute("style", "width:150px");
        sidebar.classList.remove("sidebar-secondary-hide");
        sidebar.classList.add("sidebar-secondary-show");
    } else {
        //sidebar.setAttribute("style", "width:0px");
        sidebar.classList.remove("sidebar-secondary-show");
        sidebar.classList.add("sidebar-secondary-hide");
    }

}
function SidebarChangeTab(id) {
    let oldTab = document.querySelector(".sidebar-item-active");
    let ids = "#" + id;
    let newTab = document.querySelector(ids);
    oldTab.classList.remove("sidebar-item-active");
    newTab.classList.add("sidebar-item-active");
    if (ids === "#sbChat") {
        let body = document.body;
        if (body != null) {
            body.classList.add("body-non-scroll");
        }
        if (currentState) {
            AnimateSecondarySidebar(false);
            currentState = false;
        }
        else {
            AnimateSecondarySidebar(true);
            currentState = true;
        }
        
    } else {
        let body = document.body;
        if (body != null) {
            body.classList.remove("body-non-scroll");
        }
        if (currentState) {
            AnimateSecondarySidebar(false);
            currentState = false;
        }
    }
}
function OnNavigationTab(tabID, navigationbarID) {
    let navigationBar = document.getElementById(navigationbarID);
    if (navigationBar != null) {
        RemoveActiveFromSecondaryNavbar();
        RemoveActiveFromMainNavbar();

        let child = document.querySelector("#" + tabID);
        if (child != null) {
            child.classList.add(navigationbarID === "primaryNavbar" ? "navbar-item-active" : "sidebar-item-active");
        }
    }
}
function RemoveActiveFromMainNavbar() {
    let activeChild = document.querySelector(".navbar-item-active");
    if (activeChild != null) {
        activeChild.classList.remove("navbar-item-active");
    }
}
function RemoveActiveFromSecondaryNavbar() {
    let activeChild = document.querySelector(".sidebar-item-active");
    if (activeChild != null) {
        activeChild.classList.remove("sidebar-item-active");
    }
}


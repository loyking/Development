#!/usr/bin/env/python3
#-*-coding:utf-8-*-

from requests_html import HTMLSession


if __name__ == "__main__":
    '''
    session = HTMLSession()
    r = session.get('http://www.gz5zx.com/Portal/Index?id=Main_SchoolProfilesss&sId=Sub_Dynamicss&cId=E2074B8578834B7C81356B462EA8576D')
    div = r.html.find("div[class='kuang3']")
    li = div[0].find('li')
    for item in li:
        print(item.text)
    '''
    session = HTMLSession()
    r = session.get('https://www.cnblogs.com/loyking/p/9905215.html')
    print(r.text)





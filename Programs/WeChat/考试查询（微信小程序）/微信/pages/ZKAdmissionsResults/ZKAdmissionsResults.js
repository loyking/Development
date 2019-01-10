const app = getApp();

Page({
  data: {
    SchoolName:""
  },
  onLoad:function(option){
    this.setData({
      SchoolName: option.SchoolName
    })
  },
  ContinueQuery: function () {
    wx.navigateTo({
      url: '../ZKQuery/ZKQuery',
    })
  },
  Index: function () {
    wx.navigateTo({
      url: '../index/index',
    })
  }

})
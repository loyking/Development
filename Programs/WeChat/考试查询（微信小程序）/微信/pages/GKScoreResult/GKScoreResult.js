const app = getApp();

//高考成绩结果
import GkScore from '../Template/GkScore.js'
//考生信息对象
import ExamineeInfo from '../Template/ExamineeInfo.js'


Page({
  data:{
    QueryWorL:'',  
    ChineseScore:0,
    MathematicsScore:0,
    EnglishScore:0,
    ComprehensiveScore:0,
    SumScore:0
  },
  onLoad:function(){
    this.setData({
      QueryWorL: GkScore.data.Type,
      ChineseScore: GkScore.data.ChineseScore,
      MathematicsScore: GkScore.data.MathematicsScore,
      EnglishScore: GkScore.data.EnglishScore,
      ComprehensiveScore: GkScore.data.ComprehensiveScore,
      SumScore: GkScore.data.SumScore
    })
  },
  ContinueQuery:function(){
    wx.navigateTo({
      url: '../GKQuery/GKQuery',
    })
  },
  scoreQuery:function(){

    ExamineeInfo.data.QueryType=3
    wx.request({
      url: 'http://localhost:9001/Home/Query',
      type: "GET",
      data: ExamineeInfo.data,
      success: function (model) {
        var data = model.data;
        //检测接口是否带有正确返回值（不正确返回查询无结果页面）
        if (data.Existence) {
          wx.navigateTo({
            url: '../GKAdmissionsResult/GKAdmissionsResult?Batch=' + data.Batch + "&Category=" + data.Category + "&SchoolName=" + data.SchoolName
          })
        }
        else {
          wx.navigateTo({
            url: '../NoQuery/NoQuery',
          })
        }
      }
    })


    
  }
})